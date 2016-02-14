"use strict";

class RatingViewModel {
    private id: number;
    private currentUserVote: number;

    rate: KnockoutObservable<number>;

    constructor(id: number, rate: number, currentUserVote: number) {
        this.id = id;
        this.currentUserVote = currentUserVote;
        this.rate = ko.observable(rate);
    }

    voteUp() {
        this.rate(this.rate() + 1);
    }

    voteDown() {
        this.rate(this.rate() - 1);
    }
}
