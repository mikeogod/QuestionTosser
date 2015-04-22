var QuestionTosser = QuestionTosser || {};
QuestionTosser.HideAll = function () {
    $("#main-logo, #question-panel, #professor-panel, #student-panel, #questions-panel, #student-login-panel," +
        "#professor-login-panel, #register-panel, #about-panel, #logout-panel").hide();
            
};
QuestionTosser.HideAllTransition = function (callback) {
    if (typeof callback === 'undefined') callback = function () { alert(); };
    var displayedElements=$("#question-panel, #professor-panel, #student-panel, #questions-panel, #student-login-panel," +
        "#professor-login-panel, #register-panel, #about-panel, #logout-panel").filter(function (index, domObj) {
            return $(domObj).css('display') !== 'none';
        });
    
    displayedElements.each(function (index, domObj) {
        if (index !== displayedElements.length - 1)
        {
            $(domObj).hide({ effect: 'blind', easing: 'linear', duration: 500 });
        }
        else /*(index == elementCount - 1)*/ {
            $(domObj).hide({
                effect: 'blind', easing: 'linear', duration: 500, complete: function () {
                    $("#main-logo").hide({ effect: 'fade', easing: 'linear', duration: 500, complete: callback });
                }
            });
        }
    });

};
QuestionTosser.AnonymousPage = function () {
    QuestionTosser.HideAll();
    $("#main-logo").show();
    $("#about-panel").show();
    $("#student-login-panel").show();
    $("#professor-login-panel").show();
    $("#register-panel").show();
};
QuestionTosser.StudentPage = function () {
    QuestionTosser.HideAll();
    $("#student-panel").show();
    $("#logout-panel").show();
};
QuestionTosser.ProfessorPage = function () {
    QuestionTosser.HideAll();
    $("#professor-panel").show();
    $("#professor-panel input[name='end-class']").hide();
    $("#logout-panel").show();
};
QuestionTosser.JoinSucceed = function () {
    //$("#student-panel input[name='code']").hide();
    //$("#student-panel input[name='submit']").hide();
    $("#student-panel form").hide();
    $("#question-panel").show();
};
QuestionTosser.StartSucceed = function () {
    $("#professor-panel input[name='classname']").hide();
    $("#professor-panel input[name='code']").hide();
    $("#professor-panel input[name='start-class']").hide();
    $("#professor-panel input[name='end-class']").show();
    $("#questions-panel").show();
};
QuestionTosser.EndSucceed = function () {
    $("#professor-panel input[name='classname']").show();
    $("#professor-panel input[name='code']").show();
    $("#professor-panel input[name='start-class']").show();
    $("#professor-panel input[name='end-class']").hide();
    $("#questions-panel").hide();
};
QuestionTosser.ClearLoginRegisterFields = function () {
    $("#register-panel input[type='text']").val("");
    $("#register-panel input[type='password']").val("");
    $("#student-login-panel input[type='text']").val("");
    $("#student-login-panel input[type='password']").val("");
    $("#professor-login-panel input[type='text']").val("");
    $("#professor-login-panel input[type='password']").val("");
};

