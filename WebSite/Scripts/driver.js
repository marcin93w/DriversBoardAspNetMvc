"use strict";
var RatingViewModel = (function () {
    function RatingViewModel(id, rate, currentUserVote) {
        this.id = id;
        this.currentUserVote = ko.observable(currentUserVote);
        this.rate = ko.observable(rate);
    }
    RatingViewModel.prototype.isVotedUp = function () {
        return this.currentUserVote() > 0;
    };
    RatingViewModel.prototype.isVotedDown = function () {
        return this.currentUserVote() < 0;
    };
    RatingViewModel.prototype.getTitleUp = function () {
        return this.isVotedUp() ? 'Cofnij głos' : 'Mocne';
    };
    RatingViewModel.prototype.getTitleDown = function () {
        return this.isVotedDown() ? 'Cofnij głos' : 'Nie nadaje się';
    };
    RatingViewModel.prototype.voteUp = function () {
        var _this = this;
        switch (this.currentUserVote()) {
            case -1:
                this.vote('ChangeRateToUp', function () { return _this.currentUserVote(1); });
                break;
            case 0:
                this.vote('RateUp', function () { return _this.currentUserVote(1); });
                break;
            case 1:
                this.vote('ClearRatingUp', function () { return _this.currentUserVote(0); });
                break;
        }
    };
    RatingViewModel.prototype.voteDown = function () {
        var _this = this;
        switch (this.currentUserVote()) {
            case -1:
                this.vote('ClearRatingDown', function () { return _this.currentUserVote(0); });
                break;
            case 0:
                this.vote('RateDown', function () { return _this.currentUserVote(-1); });
                break;
            case 1:
                this.vote('ChangeRateToDown', function () { return _this.currentUserVote(-1); });
                break;
        }
    };
    ///TODO change to promise
    RatingViewModel.prototype.vote = function (method, onSuccess) {
        var _this = this;
        var url = 'http://localhost:5185/api/itemsrating/';
        url += method;
        url += '/' + this.id;
        $.ajax(url)
            .done(function (data) {
            var currentRate = parseInt(data);
            if (isNaN(currentRate)) {
                _this.displayVotingError(data);
            }
            else {
                _this.rate(currentRate);
                onSuccess();
            }
        })
            .fail(function (error) {
            if (error.status === 401) {
                _this.displayUnauthirizedError();
            }
            else {
                _this.displayVotingError(error);
            }
        });
    };
    RatingViewModel.prototype.displayVotingError = function (error) {
        console.error(error.Message || error.message || error);
        alert("Nie udało się zagłosować, spróbuj ponownie.");
    };
    RatingViewModel.prototype.displayUnauthirizedError = function () {
        alert('Musisz się zalogować żeby oddać głos.');
    };
    return RatingViewModel;
})();
//# sourceMappingURL=driver.js.map