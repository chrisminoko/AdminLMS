jQuery(function ($) {
    $("form[name='smsPaymentRequest']").validate({
        rules: {
            mobileNumber: {
                required: true,
                regex_mobile_number: /^0(6|7|8){1}[0-9]{1}[0-9]{7}$/
            },
            amount: {
                regex_amount: /^-?\d+\.?\d*$/,
                required: true
            },
            reference: {
                required: true,
                maxlength: 20,
                minlength: 1
            },
        },
        messages: {
            mobileNumber: "Please enter a valid Mobile Number.",
            amount: "Please enter a valid amount.",
            reference: "Please enter a valid Reference.",
        },
        submitHandler: function (form) {
            form.submit();
        }
    });

    $.validator.addMethod("regex_mobile_number", function (value, element, regexpr) {
        return regexpr.test(value);
    }, "Please enter a valid South African Mobile Number.");

    $.validator.addMethod("regex_amount", function (value, element, regexpr) {
        return regexpr.test(value) && value >= 0.01;
    }, "Please enter a valid amount.");

});