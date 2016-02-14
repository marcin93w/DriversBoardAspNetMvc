"use strict";
var RatingViewModel = (function () {
    function RatingViewModel(id, rate, currentUserVote) {
        this.id = id;
        this.currentUserVote = currentUserVote;
        this.rate = ko.observable(rate);
    }
    RatingViewModel.prototype.voteUp = function () {
        this.rate(this.rate() + 1);
    };
    RatingViewModel.prototype.voteDown = function () {
        this.rate(this.rate() - 1);
    };
    return RatingViewModel;
})();
//# sourceMappingURL=driver.js.map