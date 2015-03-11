var QuestionTosser = QuestionTosser || {};
QuestionTosser.HideAll = function () {
    $("#question-panel, #professor-panel, #student-panel, #questions-panel, #student-login-panel,"+ 
        "#professor-login-panel, #register-panel, #about-panel, #logout-panel").hide();
};
QuestionTosser.AnonymousPage = function () {
    QuestionTosser.HideAll();
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
    $("#student-panel input[name='code']").hide();
    $("#student-panel input[name='submit']").hide();
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

