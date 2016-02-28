"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var VotingViewModel = (function () {
    function VotingViewModel(id, rate, currentUserVote) {
        this.votingApiUrl = 'http://localhost:5185/api/' + this.getControllerName() + '/';
        this.id = id;
        this.currentUserVote = ko.observable(currentUserVote);
        this.rate = ko.observable(rate);
    }
    VotingViewModel.prototype.isVotedUp = function () {
        return this.currentUserVote() > 0;
    };
    VotingViewModel.prototype.isVotedDown = function () {
        return this.currentUserVote() < 0;
    };
    VotingViewModel.prototype.getTitleUp = function () {
        return this.isVotedUp() ? 'Cofnij głos' : 'Mocne';
    };
    VotingViewModel.prototype.getTitleDown = function () {
        return this.isVotedDown() ? 'Cofnij głos' : 'Nie nadaje się';
    };
    VotingViewModel.prototype.voteUp = function () {
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
    VotingViewModel.prototype.voteDown = function () {
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
    VotingViewModel.prototype.vote = function (method, onSuccess) {
        var _this = this;
        var url = this.votingApiUrl;
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
    VotingViewModel.prototype.displayVotingError = function (error) {
        console.error(error.Message || error.message || error);
        alert("Nie udało się zagłosować, spróbuj ponownie.");
    };
    VotingViewModel.prototype.displayUnauthirizedError = function () {
        alert('Musisz się zalogować żeby oddać głos.');
    };
    return VotingViewModel;
})();
var ItemsVotingViewModel = (function (_super) {
    __extends(ItemsVotingViewModel, _super);
    function ItemsVotingViewModel() {
        _super.apply(this, arguments);
    }
    ItemsVotingViewModel.prototype.getControllerName = function () {
        return 'itemsrating';
    };
    return ItemsVotingViewModel;
})(VotingViewModel);
var CommentsVotingViewModel = (function (_super) {
    __extends(CommentsVotingViewModel, _super);
    function CommentsVotingViewModel() {
        _super.apply(this, arguments);
    }
    CommentsVotingViewModel.prototype.getControllerName = function () {
        return 'commentsrating';
    };
    return CommentsVotingViewModel;
})(VotingViewModel);
//# sourceMappingURL=RatingViewModel.js.map