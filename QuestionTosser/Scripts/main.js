var QuestionTosser = QuestionTosser || {};

if (typeof showEffect !== 'undefined' || typeof hideEffect !== 'undefined' || typeof easingType !== 'undefined' || typeof durationLen !== 'undefined') {
    throw new Error("Name collision happened.");
}
var showEffect = showEffect || "blind";
var hideEffect = hideEffect || "blind";
var easingType = easingType || "swing";
var durationLen = durationLen || 500;
QuestionTosser.HideAll = function () {
    $("#main-logo, #question-panel, #professor-panel, #student-panel, #questions-panel, #endclass-panel, #student-login-panel," +
        "#professor-login-panel, #register-panel, #about-panel, #logout-panel").hide();
            
};
QuestionTosser.HideAllWithTransition = function (callback) {
    if (typeof callback === 'undefined') callback = function () { };
    //Filter out all the hidden panels, keeping only visible ones
    var displayedElements = $("#question-panel, #professor-panel, #student-panel, #questions-panel, #endclass-panel, #student-login-panel," +
        "#professor-login-panel, #register-panel, #about-panel, #logout-panel").filter(function (index, domObj) {
            return $(domObj).css('display') !== 'none';
        });
    
    displayedElements.each(function (index, domObj) {
        if (index !== displayedElements.length - 1)
        {
            $(domObj).hide({ effect: hideEffect, easing: easingType, duration: durationLen });
        }
        else /*(index == elementCount - 1)*/ {
            $(domObj).hide({
                effect: hideEffect, easing: easingType, duration: durationLen, complete: function () {
                    if ($("#main-logo").css('display') !== 'none') {
                        $("#main-logo").hide({ effect: hideEffect, easing: easingType, duration: durationLen, complete: callback });
                    }
                    else {
                        callback();
                    }
                }
            });
        }
    });
    if (displayedElements.length === 0) {
        callback();
    }
    
};
QuestionTosser.AnonymousPage = function () {
    QuestionTosser.HideAllWithTransition(function () {
        $("#main-logo").show({ effect: showEffect, easing: easingType, duration: durationLen });
        $("#about-panel").show({ effect: showEffect, easing: easingType, duration: durationLen });
        $("#student-login-panel").show({ effect: showEffect, easing: easingType, duration: durationLen });
        $("#professor-login-panel").show({ effect: showEffect, easing: easingType, duration: durationLen });
        $("#register-panel").show({ effect: showEffect, easing: easingType, duration: durationLen });
    });
    
};
QuestionTosser.StudentPage = function () {
    QuestionTosser.HideAllWithTransition(function () {
        $("#student-panel").show({ effect: showEffect, easing: easingType, duration: durationLen });
        $("#logout-panel").show({ effect: showEffect, easing: easingType, duration: durationLen });
    });
   
};
QuestionTosser.ProfessorPage = function () {
    QuestionTosser.HideAllWithTransition(function () {
        $("#professor-panel").show({ effect: showEffect, easing: easingType, duration: durationLen });
        $("#logout-panel").show({ effect: showEffect, easing: easingType, duration: durationLen });
    });
    
};
QuestionTosser.JoinSucceed = function () {
    QuestionTosser.HideAllWithTransition(function () {
        //$("#student-panel").hide({ effect: hideEffect, easing: easingType, duration: durationLen });
        $("#question-panel").show({ effect: showEffect, easing: easingType, duration: durationLen });
        $("#logout-panel").show({ effect: showEffect, easing: easingType, duration: durationLen });
    });
};
QuestionTosser.StartSucceed = function () {
    QuestionTosser.HideAllWithTransition(function () {
        //$("#professor-panel").hide();
        $("#endclass-panel").show({ effect: showEffect, easing: easingType, duration: durationLen });
        $("#questions-panel").show({ effect: showEffect, easing: easingType, duration: durationLen });
    });
};
QuestionTosser.EndSucceed = function () {
    QuestionTosser.HideAllWithTransition(function () {
        $("#professor-panel").show({ effect: showEffect, easing: easingType, duration: durationLen });
        //$("#endclass-panel").hide();
        //$("#questions-panel").hide();
        $("#logout-panel").show({ effect: showEffect, easing: easingType, duration: durationLen });
    });
};
QuestionTosser.ClearLoginRegisterFields = function () {
    $("#register-panel input[type='text']").val("");
    $("#register-panel input[type='password']").val("");
    $("#student-login-panel input[type='text']").val("");
    $("#student-login-panel input[type='password']").val("");
    $("#professor-login-panel input[type='text']").val("");
    $("#professor-login-panel input[type='password']").val("");
};

