"use strict";

class RatingViewModel {
    private id: number;
    private currentUserVote: KnockoutObservable<number>;

    rate: KnockoutObservable<number>;

    constructor(id: number, rate: number, currentUserVote: number) {
        this.id = id;
        this.currentUserVote = ko.observable(currentUserVote);
        this.rate = ko.observable(rate);
    }

    isVotedUp(): boolean {
        return this.currentUserVote() > 0;
    }

    isVotedDown(): boolean {
        return this.currentUserVote() < 0;
    }

    getTitleUp(): string {
        return this.isVotedUp() ? 'Cofnij głos' : 'Mocne';
    }

    getTitleDown(): string {
        return this.isVotedDown() ? 'Cofnij głos' : 'Nie nadaje się';
    }

    voteUp() {
        switch(this.currentUserVote()) {
            case -1:
                this.vote('ChangeRateToUp', () => this.currentUserVote(1));
                break;
            case 0:
                this.vote('RateUp', () => this.currentUserVote(1));
                break;
            case 1:
                this.vote('ClearRatingUp', () => this.currentUserVote(0));
                break;
        }
    }

    voteDown() {
        switch (this.currentUserVote()) {
            case -1:
                this.vote('ClearRatingDown', () => this.currentUserVote(0));
                break;
            case 0:
                this.vote('RateDown', () => this.currentUserVote(-1));
                break;
            case 1:
                this.vote('ChangeRateToDown', () => this.currentUserVote(-1));
                break;
        }
    }

    ///TODO change to promise
    private vote(method, onSuccess) {
        var url = 'http://localhost:5185/api/itemsrating/';
        url += method;
        url += '/' + this.id;
        $.ajax(url)
            .done((data) => {
                var currentRate: number = parseInt(data);
                if (isNaN(currentRate)) {
                    this.displayVotingError(data);
                } else {
                    this.rate(currentRate);
                    onSuccess();
                }
            })
            .fail((error) => {
                if (error.status === 401) {
                    this.displayUnauthirizedError();
                } else {
                    this.displayVotingError(error);
                }
            });
    }

    private displayVotingError(error) {
        console.error(error.Message || error.message || error);
        alert("Nie udało się zagłosować, spróbuj ponownie.");
    }

    private displayUnauthirizedError() {
        alert('Musisz się zalogować żeby oddać głos.');
    }
}
