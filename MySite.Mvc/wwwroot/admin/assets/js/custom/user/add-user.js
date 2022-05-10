"use strict";

// Class Definition
var KTAddUser = function () {
    // Private Variables
    var _wizardEl;
    var _formEl;
    var _wizardObj;
    var _avatar;
    var _validations = [];

    // Private Functions
    var _initWizard = function () {
        // Initialize form wizard
        _wizardObj = new KTWizard(_wizardEl, {
            startStep: 1, // initial active step number
            clickableSteps: false  // allow step clicking
        });

        // Validation before going to next page
        _wizardObj.on('change', function (wizard) {
            if (wizard.getStep() > wizard.getNewStep()) {
                return; // Skip if stepped back
            }

            // Validate form before change wizard step
            var validator = _validations[wizard.getStep() - 1]; // get validator for currnt step

            if (validator) {
                validator.validate().then(function (status) {
                    if (status == 'Valid') {
                        wizard.goTo(wizard.getNewStep());

                        KTUtil.scrollTop();
                    } else {
                        Swal.fire({
                            text: "Bazý alanlar hatalý veya boþ geçilmiþtir. Lütfen kontrol ediniz.",
                            icon: "error",
                            buttonsStyling: false,
                            confirmButtonText: "Tamam!",
                            customClass: {
                                confirmButton: "btn font-weight-bold btn-light"
                            }
                        }).then(function () {
                            KTUtil.scrollTop();
                        });
                    }
                });
            }

            return false;  // Do not change wizard step, further action will be handled by he validator
        });

        // Change event
        _wizardObj.on('changed', function (wizard) {
            KTUtil.scrollTop();
        });

        // Submit event
        _wizardObj.on('submit', function (wizard) {
            Swal.fire({
                text: "Kullanýcý kayýt edilsin mi?",
                icon: "success",
                showCancelButton: true,
                buttonsStyling: false,
                confirmButtonText: "Evet",
                cancelButtonText: "Hayýr",
                customClass: {
                    confirmButton: "btn font-weight-bold btn-primary",
                    cancelButton: "btn font-weight-bold btn-default"
                }
            }).then(function (result) {
                if (result.value) {
                    _formEl.submit(); // Submit form
                } else if (result.dismiss === 'cancel') {
                    Swal.fire({
                        text: "Kullanýcý kayýt edilmedi.",
                        icon: "error",
                        buttonsStyling: false,
                        confirmButtonText: "Tamam",
                        customClass: {
                            confirmButton: "btn font-weight-bold btn-primary",
                        }
                    });
                }
            });
        });
    }

    var _initValidations = function () {
        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/

        // Validation Rules For Step 1
        _validations.push(FormValidation.formValidation(
            _formEl,
            {
                fields: {
                    username: {
                        validators: {
                            notEmpty: {
                                message: 'Kullanýcý adý alaný boþ geçilemez.'
                            },
                            stringLength: {
                                min: 6,
                                max: 50,
                                message: 'Kullanýcý adý minimum 6 ve maksimum 50 karakter olmalýdýr.'
                            },
                            regexp: {
                                regexp: /^[a-zA-Z0-9_.]+$/,
                                message: 'Kullanýcý adý yalnýzca alfabetik, sayý, nokta ve alt çizgiden oluþabilir.'
                            }
                        }
                    },
                    firstname: {
                        validators: {
                            notEmpty: {
                                message: 'Ad alaný boþ geçilemez.'
                            }
                        }
                    },
                    lastname: {
                        validators: {
                            notEmpty: {
                                message: 'Soyad alaný boþ geçilemez.'
                            }
                        }
                    },
                    email: {
                        validators: {
                            notEmpty: {
                                message: 'E-Posta adresi boþ geçilemez.'
                            },
                            emailAddress: {
                                message: 'Geçerli bir mail adresi giriniz.'
                            }
                        }
                    },
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    // Bootstrap Framework Integration
                    bootstrap: new FormValidation.plugins.Bootstrap({
                        //eleInvalidClass: '',
                        eleValidClass: ''
                    }),
                    icon: new FormValidation.plugins.Icon({
                        valid: 'fa fa-check',
                        invalid: 'fa fa-times',
                        validating: 'fa fa-refresh'
                    })
                }
            }
        ));

        _validations.push(FormValidation.formValidation(
            _formEl,
            {
                fields: {
                    // Step 2
                    password: {
                        validators: {
                            notEmpty: {
                                message: 'Þifre alaný boþ geçilemez.'
                            }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    // Bootstrap Framework Integration
                    bootstrap: new FormValidation.plugins.Bootstrap({
                        //eleInvalidClass: '',
                        eleValidClass: '',
                    })
                }
            }
        ));

    }

    var _initAvatar = function () {
        _avatar = new KTImageInput('kt_user_add_avatar');
    }

    return {
        // public functions
        init: function () {
            _wizardEl = KTUtil.getById('kt_wizard');
            _formEl = KTUtil.getById('kt_form');

            _initWizard();
            _initValidations();
            _initAvatar();
        }
    };
}();

jQuery(document).ready(function () {
    KTAddUser.init();
});

var url = '/Admin/Medya/GetAllUploadsPartial/';
var modalUploadList = $('#modalUploadList');
$(document).on('click',
    '#uploadChoose',
    function (event) {
        event.preventDefault();
        $.get(url).done(function (data) {
            modalUploadList.html(data);
            modalUploadList.find('.modal').modal('show');
        }).fail(function (err) {
            toastr.error(`${err.responseText}`, 'Hata!');
        });
    });

$('#uploadRemove').click(function (event) {
    event.preventDefault();
    imageDiv.css('background-image', "");
});