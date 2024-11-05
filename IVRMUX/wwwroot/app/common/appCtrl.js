
app.controller("appCtrl", ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile',
    function ($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile) {

        //right click and console has been disabled
        document.addEventListener('contextmenu', event => event.preventDefault());
        $(document).keydown(function (event) {
            if (event.keyCode == 123) {
               // return false;
                event.preventDefault();
            }
            else if (event.ctrlKey && event.shiftKey && event.keyCode == 73) {
                return false;
            }
            else if (event.ctrlKey && event.shiftKey && event.keyCode == 'I'.charCodeAt(0)) {
                return false;
            }
            else if (event.ctrlKey && event.shiftKey && event.keyCode == 'J'.charCodeAt(0)) {
                return false;
            }
            else if (event.ctrlKey && event.keyCode == 'U'.charCodeAt(0)) {
                return false;
            }
            else if (event.ctrlKey && event.keyCode == '123'.charCodeAt(0)) {
                return false;
            }
            else if (event.ctrlKey && event.shiftKey && event.keyCode == 73) { // Prevent Ctrl+Shift+I        
                return false;
            }

        });

        //document.addEventListener('contextmenu', event => event.preventDefault());
        $rootScope.theme = appSettings.theme;
        $rootScope.layout = appSettings.layout;

        $('#myModalswal').modal('hide');

        $scope.ProspectuseScreen = true;
        $scope.PaymentMode = false;
        $scope.reg = {};
        var configsettings = "";

        $scope.modulelistarry = {};
        $scope.pagelistarray = {};

        $scope.daspgenme = {};
        $scope.clickonmodules = function () {
            swal("Please use Left MenuBar to navigate and access Pages!!!");
        };
        var transnumconfigsettings1 = "";
        var vm = this;
        $scope.pageTitle = "Home";
        $scope.pageTitle = $cookieStore.get("pagetitle");
        $scope.ModuleName = $cookieStore.get("ModuleName");
        $scope.SubMenuName = $cookieStore.get("SubMenuName");
        //$scope.pageTitle = $cookieStore.get("pagetitle") ? $cookieStore.get("pagetitle") : "Home Page";
        $scope.setpagetitle = function (title) {
            $cookieStore.put("pagetitle", title);
            $scope.pageTitle = $cookieStore.get("pagetitle");
        };
        var dashboard = "";
        // set Module name
        $scope.setModuleName = function (title) {
            if (title == "Dashboard") {
                $state.reload();
                $cookieStore.remove('SubMenuName');
                $cookieStore.remove('SubMenuName');
                $cookieStore.remove('pagetitle');
                $cookieStore.put("ModuleName", "Dashboard");
                $scope.ModuleName = $cookieStore.get("ModuleName");
                $scope.SubMenuName = $cookieStore.get("SubMenuName");
                $scope.pageTitle = $cookieStore.get("pagetitle");
            } else {
                $scope.moduleListsubmenu = $scope.modulelistarry;
                $cookieStore.put("ModuleName", title);
                $scope.ModuleName = $cookieStore.get("ModuleName");
            }
        };

        //for contact us toll free number:
        $scope.contactus = function () {
            swal("Contact Us on Toll Free No. 01206901888");
          
        }

        $scope.translatedtxt;
        
       
        $scope.OntextChage = function () {
            $scope.translatedtxt='t'
            //$scope.TexttoSpeak = "";
            var apiKey = ""; var url = "";
            if ($scope.Sourcelong != null && $scope.Targetlong != null && $scope.Sourcelong != "" && $scope.Targetlong != "") {                                            
                 apiKey = 'AIzaSyCqYiUTcgHWAVn5brSRNtAOB1b1EGam-DQ'; // Replace with your Google Cloud API key
                  url = `https://translation.googleapis.com/language/translate/v2?key=${apiKey}`;
                const data = {
                    q: $scope.drremarks,
                    source: $scope.Sourcelong,
                    target: $scope.Targetlong,
                };
                fetch(url, {
                    method: 'POST',
                    body: JSON.stringify(data),
                    headers: {
                        'Content-Type': 'application/json',
                    },
                })
                    .then(response => response.json())
                    .then(data => {                      
                        $scope.TexttoSpeak = data.data.translations[0].translatedText;
                    })
                    .catch(error => {
                        console.error(error);      
                        $scope.TexttoSpeak = "";
                        $scope.drremarks = "";
                        swal("Text  Is Not Entered !  ")
                    });
            }
            
        }
       
        $scope.savemsg = function () {
            $scope.submitted = true;
            $scope.chatCompletion = "";
            if ($scope.chatgptmessage !="") {
                var data = {
                    "message": $scope.chatgptmessage,
                }
                apiService.create("Chatgpt/chatgpt", data).then(function (promise) {
                    $scope.chatgptmessage = "";
                    if (promise.chatCompletion != null && promise.chatCompletion != "") {
                        $scope.chatCompletion = promise.chatCompletion;
                    }
                    else {
                        swal("Invalid Input !");
                    }
                })
            }
        };

        $scope.speakTexttwo = function () {
            
            var textToSpeak = $scope.drremarks;
            var speechOutput = document.getElementById("speechOutput");

            if ('speechSynthesis' in window) {
                const synth = window.speechSynthesis;
                const utterance = new SpeechSynthesisUtterance(textToSpeak);
                synth.speak(utterance);
                speechOutput.textContent = `Speaking: ${textToSpeak}`;
            }
            else {
                speechOutput.textContent = 'Text-to-speech not supported in this browser.';
            }
        }
        //speakTexthree
        $scope.speakTexthree = function () {

            var textToSpeak = $scope.Flashs;
            var speechOutput = document.getElementById("speechOutput");

            if ('speechSynthesis' in window) {
                const synth = window.speechSynthesis;
                const utterance = new SpeechSynthesisUtterance(textToSpeak);
                synth.speak(utterance);
                speechOutput.textContent = `Speaking: ${textToSpeak}`;
            }
            else {
                speechOutput.textContent = 'Text-to-speech not supported in this browser.';
            }
        }

        //copy translated text
       $scope.copytxt= function () {
            var copyText = document.getElementById("translatedtxt");
            copyText.select();
            copyText.setSelectionRange(0, 99999); 
            navigator.clipboard.writeText(copyText.value);
        }

        //download translated text
        $scope.downloadtext = function (){
             const link = document.createElement("a");
            var dwntxt = document.getElementById("translatedtxt").innerHTML
            var file = new Blob([dwntxt], { type: 'text/plain' });
            link.href = URL.createObjectURL(file);
            link.download = "translatedtxt.txt";
            link.click();
            URL.revokeObjectURL(link.href);
        };

        //pdf Code
        $scope.convertToPDF = function () {
            var fileInput = document.getElementById('fileInput');
            var file = fileInput.files[0];

            if (file) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    try {
                        var content = e.target.result;
                        var fileType = file.type;

                        var docDefinition;

                        if (fileType.includes('image')) {
                            docDefinition = {
                                content: [
                                    { image: content, width: 500 }
                                ]
                            };
                        }

                        else if (fileType === 'application/pdf') {
                            docDefinition = {
                                content: [
                                    { text: 'PDF content goes here.' }
                                ]
                            };
                        }
                        else if (fileType.includes('text') || fileType === 'application/octet-stream') {
                            var textContent = e.target.result;
                            docDefinition = {
                                content: [
                                    { text: textContent }
                                ]
                            };
                        }
                        else if (fileType === 'application/msword') {
                            // For DOC files
                            mammoth.extractRawText({ arrayBuffer: e.target.result })
                                .then(function (result) {
                                    var textContent = result.value;
                                    docDefinition = {
                                        content: [
                                            { text: textContent }
                                        ]
                                    };
                                    createAndDownloadPDF(docDefinition);
                                })
                                .catch(function (error) {
                                    $scope.errorMessage = 'Error occurred while generating PDF: ' + error.message;
                                });
                            return;
                        } else {
                            throw new Error('Unsupported file type.');
                        }

                        createAndDownloadPDF(docDefinition);
                    } catch (error) {
                        $scope.errorMessage = 'Error occurred while generating PDF: ' + error.message;
                    }
                };
                reader.readAsDataURL(file);
            } else {
                $scope.errorMessage = 'Please select a file.';
            }
        };

        function createAndDownloadPDF(docDefinition) {
            var pdfDoc = pdfMake.createPdf(docDefinition);

            // Automatically download the PDF after it's generated
            pdfDoc.download('generated.pdf', function () {
                $scope.errorMessage = '';
            });
        }

        //pdf


        var HostName = location.host;
        $scope.fillpay = function (pasR_Id) {
            $window.location.href = 'http://' + HostName + '/#/app/PreadmissionOnlinePayment/';
        };
        //SubMenuName
        $scope.setSubMenuName = function (title, menuId, moduleId) {
            $cookieStore.put("SubMenuName", title);
            $scope.SubMenuName = $cookieStore.get("SubMenuName");
            $scope.pageListT = [];
            $scope.pageList = [];
            $scope.pageListM = [];
            angular.forEach($scope.pagelistarray, function (opqr1) {
                if (opqr1.menuId == menuId) {
                    $scope.pageListT.push(opqr1);
                }
            });
            angular.forEach($scope.pagelistarray, function (opqr1) {
                if (opqr1.moduleId == moduleId) {
                    $scope.pageListM.push(opqr1);
                }
            });
            if ($scope.pageListT.length >= 10) {
                $scope.pageList = $scope.pageListT;
            }
            else if ($scope.pageListT.length >= 1 && $scope.pageListT.length <= 5) {
                angular.forEach($scope.pagelistarray, function (opqr1) {
                    if ($scope.pageListM.length >= 100) {
                        if (opqr1.menuId == menuId) {
                            $scope.pageList.push(opqr1);
                        }
                    }
                    else {
                        if (opqr1.moduleId == moduleId) {
                            $scope.pageList.push(opqr1);
                        }
                    }
                });
            }
            else {
                angular.forEach($scope.pagelistarray, function (opqr1) {
                    if (opqr1.menuId == menuId) {
                        $scope.pageList.push(opqr1);
                    }
                });
            }
        };
        $scope.fetchhomepage = function () {
            var data = {
            };
            apiService.create("Login/getrolewisepage", data).then(function (promise) {
                if (promise.returnMsg != "Expired") {
                    if (promise.filldashpagemap != null && promise.filldashpagemap.length > 0) {
                        if (promise.pageexists.length > 0) {
                            $scope.daspgenme = promise.filldashpagemap[0].ivrmP_Dasboard_PageName;
                        }
                        else {
                            $scope.daspgenme = "app.homepage";
                        }
                    }
                    else {
                        $scope.daspgenme = "app.homepage";
                    }
                    $state.go($scope.daspgenme);
                } else {
                    swal("Your Session has been Expired.....Please Re-login");
                    $state.go('login');
                }
            });
        };
      
        vm.themes = [
            {
                theme: "black",
                color: "skin-black",
                title: "Dark - Black Skin",
                icon: ""
            },
            {
                theme: "black",
                color: "skin-black-light",
                title: "Light - Black Skin",
                icon: "-o"
            },
            {
                theme: "blue",
                color: "skin-blue",
                title: "Dark - Blue Skin",
                icon: ""
            },
            {
                theme: "blue",
                color: "skin-blue-light",
                title: "Light - Blue Skin",
                icon: "-o"
            },
            {
                theme: "green",
                color: "skin-green",
                title: "Dark - Green Skin",
                icon: ""
            },
            {
                theme: "green",
                color: "skin-green-light",
                title: "Light - Green Skin",
                icon: "-o"
            },
            {
                theme: "yellow",
                color: "skin-yellow",
                title: "Dark - Yellow Skin",
                icon: ""
            },
            {
                theme: "yellow",
                color: "skin-yellow-light",
                title: "Light - Yellow Skin",
                icon: "-o"
            },
            {
                theme: "red",
                color: "skin-red",
                title: "Dark - Red Skin",
                icon: ""
            },
            {
                theme: "red",
                color: "skin-red-light",
                title: "Light - Red Skin",
                icon: "-o"
            },
            {
                theme: "purple",
                color: "skin-purple",
                title: "Dark - Purple Skin",
                icon: ""
            },
            {
                theme: "purple",
                color: "skin-purple-light",
                title: "Light - Purple Skin",
                icon: "-o"
            },
        ];
        //status progress bar
        $scope.Status = [
            {
                title: "Complete Status",
                theme: "red",
                percentage: 80
            }
        ];

        //available layouts
        vm.layouts = [
            {
                name: "Boxed",
                layout: "layout-boxed"
            },
            {
                name: "Fixed",
                layout: "fixed"
            },
            {
                name: "Sidebar Collapse",
                layout: "sidebar-collapse"
            },
        ];

        //Main menu items of the dashboard
        //$scope.moduleList = [
        //    {
        //        ivrmM_ModuleName: 'Pre Admission', ivrmM_Moduleicon: 'dvr', IVRM_ModuleColor: 'color12'
        //    },

        //    {
        //        ivrmM_ModuleName: 'Admission', IVRM_Moduleicon: 'account_circle', IVRM_ModuleColor: 'color11'
        //    },
        //    {
        //        ivrmM_ModuleName: "Fees", IVRM_Moduleicon: "account_balance_wallet", IVRM_ModuleColor: "color8"
        //    },
        //    {
        //        ivrmM_ModuleName: "Exam", IVRM_Moduleicon: "edit", IVRM_ModuleColor: "color6"
        //    },
        //    {
        //        ivrmM_ModuleName: "Alumni", IVRM_Moduleicon: "school", IVRM_ModuleColor: "color9"
        //    },
        //    {
        //        ivrmM_ModuleName: "Library", IVRM_Moduleicon: "library_books", IVRM_ModuleColor: "color6"
        //    },
        //    {
        //        ivrmM_ModuleName: "Calendar of Events", IVRM_Moduleicon: "perm_contact_calendar", IVRM_ModuleColor: "color7"
        //    }

        //];
        //set the theme selected
        vm.setTheme = function (value) {
            $rootScope.theme = value;
        };


        //set the Layout in normal view
        vm.setLayout = function (value) {
            $rootScope.layout = value;
        };


        //controll sidebar open & close in mobile and normal view
        vm.sideBar = function (value) {
            if ($(window).width() <= 767) {
                if ($("body").hasClass('sidebar-open'))
                    $("body").removeClass('sidebar-open');
                else
                    $("body").addClass('sidebar-open');
            }
            else {
                if (value == 1) {
                    if ($("body").hasClass('sidebar-collapse'))
                        $("body").removeClass('sidebar-collapse');
                    else
                        $("body").addClass('sidebar-collapse');
                }
            }
        };

        //navigate to search page
        vm.search = function () {
            $state.go('app.search');
        };

        //$scope.roleType = function () {
        //    apiService.get("Login/getRole/").
        //   then(function (promise) {
        //       if (promise != "") {
        //           alert(promise);
        //           return promise;
        //           //$scope.vm.roleId = promise;
        //           //$state.go('app.homepage');
        //       }
        //   });
        //}

        var HostName = location.host;
        $scope.fillapp = function (pasR_Id) {
            var data = {
                "pasR_Id": pasR_Id
            };
            apiService.create("StudentApplication/getdashboardpage", data).then(function (promise) {
                $window.location.href = 'https://' + HostName + '/#/app/' + promise.dashboardpage + '/';
            });
        };

        $scope.GetRoleTypesnothing = function () {
            var data = {
                "pasR_Id": 1
            };
            apiService.create("Login/GetRoleTypesnothing", data).then(function (promise) {
                // $window.location.href = 'https://' + HostName + '/#/app/' + promise.dashboardpage + '/';
            });
        };

        $scope.GetRoleTypes = function () {
            $cookieStore.remove('SubMenuName');
            $cookieStore.remove('pagetitle');
            $cookieStore.put("SubMenuName", "");
            $cookieStore.put("pagetitle", "");
            $cookieStore.put("ModuleName", "Dashboard");
            //$scope.siblinglist = [];
            apiService.get("Login/getRole/").then(function (promise) {
                if (promise !== "" && promise.isSessionExpired != true) {                   
                    $scope.moduleListonly = promise.moduleListonly;
                    $scope.modulelistarry = promise.moduleList;                    
                    $scope.pagelistarray = promise.pageList;                    
                    if (promise.filldashpagemap != undefined && promise.filldashpagemap !== null && promise.filldashpagemap.length > 0) {
                        $scope.daspgenme = promise.filldashpagemap[0].ivrmP_Dasboard_PageName;                       
                    }
                    else {
                        $scope.daspgenme = "app.homepage";
                    }

                    var modalcount = 0;
                    $scope.Temp_notification = [];
                    if (promise.paymentNootificationGeneral === 0) {
                        $scope.getpaymentnotificationdetails = promise.getpaymentnotificationdetails;
                        if ($scope.getpaymentnotificationdetails !== null && $scope.getpaymentnotificationdetails.length > 0) {
                            modalcount += 1;
                            $scope.payment_content = promise.payment_content;
                            $scope.Temp_notification.push({
                                Flag: 'Payment Remainder', Message: $scope.payment_content, Remarks: '', RemainderTemplateName: 'Payment_Remainder'
                            });
                        }
                    }
                    if (modalcount > 0) {
                        $('#modalremainders').modal('show');
                    }

                    $scope.userlist = [];
                    $scope.userlist.push("abc");
                    $scope.userlist.push("def");
                   
                    if (promise.userData != undefined && promise.userData != null && promise.userData.length > 0) {
                        $scope.uName = promise.userData[0].userName;
                        localStorage.setItem("username", $scope.uName);
                        $scope.rName = promise.userData[0].roleName;
                        $scope.imgname = promise.userData[0].userImagePath;
                        $scope.Userappname = promise.userData[0].userappname;
                        $scope.Useremail = promise.userData[0].useremail;
                        $scope.usermob = promise.userData[0].usermob;
                    }
                    if (promise.studname != null) {
                        $scope.ufullName = promise.studname;
                        $scope.halfname = promise.studnamehalf;
                        $scope.halflength = promise.studnamehalf.length;
                        $scope.userlength = promise.studname.length;
                    }
                    else {
                        $scope.ufullName = null;
                    }
                    $scope.midata = promise.mIdata;
                    $scope.siblinglist = promise.siblinglist;
                    var list = promise.privileges;
                    if ($scope.rName == 'OnlinePreadmissionUser') {
                        $scope.adminmodel = false;
                        $scope.usermodel = true;
                        $scope.admissionstudent = false;
                    }
                    else if ($scope.rName == 'STUDENT' || $scope.rName == 'Student') {
                        $scope.adminmodel = false;
                        $scope.usermodel = false;
                        $scope.admissionstudent = true;
                        if (promise.chleft == "S") {
                            swal('Student Login Deactivated!!');
                            vm.logout();
                        }
                    }
                    else {
                        $scope.adminmodel = true;
                        $scope.usermodel = false;
                        $scope.admissionstudent = false;
                    }
                    localStorage.setItem("privileges", JSON.stringify(list));

                    var conlistlist = promise.configlist;
                    localStorage.setItem("configsettings", JSON.stringify(conlistlist));
                    if (conlistlist != null && conlistlist.length > 0) {
                        $scope.configurationsettings = conlistlist[0];
                    }
                    var transnumconfig = promise.transnumconfig;
                    localStorage.setItem("transnumconfigsettings", JSON.stringify(transnumconfig));

                    transnumconfigsettings1 = transnumconfig;

                    var feeconlistlist = promise.feeconfiglist;
                    localStorage.setItem("feeconfigsettings", JSON.stringify(feeconlistlist));

                    var admlistlist = promise.admissioncongigurationList;
                    localStorage.setItem("admfigsettings", JSON.stringify(admlistlist));

                    var academicyearlist = promise.fillyear;
                    localStorage.setItem("academicyearlist", JSON.stringify(academicyearlist));

                    var ivrmgeneralconfiglist = promise.ivrmconfiglist;
                    localStorage.setItem("ivrmgeneralconfiglist", JSON.stringify(ivrmgeneralconfiglist));

                    var ivrmgeneralhomepagelist = promise.filldashpagemap;
                    localStorage.setItem("ivrmgeneralhomepagelist", JSON.stringify(ivrmgeneralhomepagelist));

                    var ivrmstoragelist = promise.storagedetails;
                    localStorage.setItem("ivrmstoragelist", JSON.stringify(ivrmstoragelist));

                    var currentyear = promise.currentyear;
                    localStorage.setItem("currentyear", JSON.stringify(currentyear));

                    if (promise.fillinstition != undefined && promise.fillinstition != null && promise.fillinstition.length > 0) {
                        $scope.institutionname = promise.fillinstition[0].mI_Name;
                    }

                    if ($scope.rName == 'STUDENT' || $scope.rName == 'Student') {
                        $scope.imgsrc = promise.studentdata[0].imgnme;
                        $scope.imgname = promise.studentdata[0].imgnme;                       
                        if (promise.userImagePath !== "" && promise.userImagePath != null) {
                            $scope.imgsrc = promise.userImagePath;
                            $scope.imgname = promise.userImagePath;
                        }
                    }
                    var mandatory = promise.manadatoryfields;
                    localStorage.setItem("fieldsManadatory", JSON.stringify(mandatory));

                    if (promise.smscreditalert === 0) {
                        if (promise.smsalrtflag === true) {
                            var sms = 'SMS CREDIT BALANCE :' + ' ' + promise.rcredit;                          
                            $scope.showcreditsms(sms);                           
                        }
                    }
                }
                else {
                    window.location.href = "http://localhost:57606/#/login/";

                }
            });
        };
        $scope.showcreditsms = function (stringdisplay, SweetAlert) {
            swal({
                html: true,
                title: "<span style='color:red'>REMINDER!!!! </span> <br/>" + "<span style='color:green;font-size: 20px'>" + stringdisplay + "</span>",
                //text: stringdisplay,
                text: "<span style='color:red'> <b>Your SMS Credit Balance is low .\n Please recharge for uninterrupted sms services</b></span>",
                //type: "input",
                showCancelButton: false,
                closeOnConfirm: false,
                inputPlaceholder: "Enter Remarks",
                confirmButtonText: "OK"
            }, function () {
                var data = {
                    "MI_Id": 1,
                };
                apiService.create("InstitutionUserMapping/setsmscreditsession", data).then(function (promise) {
                    if (promise !== null) {
                        swal.close();
                    }
                });
            });
        };

        $scope.DeletRecord = function (stringdisplay, SweetAlert) {
            swal({
                //html: true,
                title: "Payment Subscription !",
                //text: stringdisplay,
                text: stringdisplay,
                type: "input",
                showCancelButton: false,
                closeOnConfirm: false,
                allowEscapeKey: false,
                closeOnClickOutside: false,
                inputPlaceholder: "Enter Remarks",
                confirmButtonText: "OK"
            }, function (inputValue) {
                if (inputValue === false) return false;
                if (inputValue === "") {
                    swal.showInputError("You need to write something!");
                    return false;
                }
                if (inputValue !== "") {
                    var data = {
                        "subscriptionremarks": inputValue,
                        "paymentsubscriptiontype": "PaymentNootificationGeneral"

                    };
                    apiService.create("InstitutionUserMapping/savepaymentremarks", data).then(function (promise) {
                        if (promise !== null) {
                            swal("Remarks Captured");
                        }
                    });
                }
            });
        };

        console.log('getting in to the app controller');

        vm.logout = function () {
            $cookieStore.remove('pagetitle');
            $cookieStore.put("pagetitle", "Home");
            $cookieStore.remove('IsLogged');
            apiService.getURI("Login/clearsession", 1).then(function (promise) {
                //$state.go("login");
                if (promise.subDomainNamelogout != null && promise.subDomainNamelogout != "") {
                    window.location.href = "http://localhost:57606/#/login/" + promise.subDomainNamelogout;
                }
                else {
                    window.location.href = "http://localhost:57606/#/login/";
                }
            });
        };

        $scope.Back = function () {

            $scope.PaymentMode = false;
            $scope.ProspectuseScreen = true;
            // $scope.cancel();
            //  $state.reload();
        };

        vm.InstituteChange = function (mI_Id) {

            var id = mI_Id;

            $cookieStore.remove('SubMenuName');
            $cookieStore.remove('pagetitle');
            $cookieStore.put("SubMenuName", "");
            $cookieStore.put("pagetitle", "");

            $cookieStore.put("ModuleName", "Dashboard");



            apiService.getURI("Login/getRoleoForChangeInstitute/", id).then(function (promise) {

                $scope.fetchhomepage();
                $state.reload();

            });
        }

        vm.siblingChange = function (amsT_Id) {

            var id = amsT_Id;

            $cookieStore.remove('SubMenuName');
            $cookieStore.remove('pagetitle');
            $cookieStore.put("SubMenuName", "");
            $cookieStore.put("pagetitle", "");

            $cookieStore.put("ModuleName", "Dashboard");



            apiService.getURI("Login/getRoleoForChangesibling/", id).then(function (promise) {

                $scope.fetchhomepage();
                $state.reload();

            });
        }

        vm.clicksibling = function () {

            console.log($scope.siblinglist);

            apiService.getURI("Login/getsiblinglist", 0).then(function (promise) {

                $scope.viewpopup = "false";

                $scope.siblings = promise.siblinglist;

                if (promise.siblinglist.length > 1) {
                    $('#siblingid').modal('show');
                }
                //else if (promise.mIdata.length == 1) {
                //    $scope.GetRoleTypes();
                //}
                else {
                    $scope.institution = '#';
                }
            });
        };

        vm.institute = function () {
            apiService.getURI("Login/getMIdataMaster", 0).then(function (promise) {

                $scope.viewpopup = "false";

                $scope.MIdata = promise.mIdata;

                if (promise.mIdata.length > 1) {
                    $('#institution').modal('show');
                }
                else if (promise.mIdata.length == 1) {
                    $scope.GetRoleTypes();
                }
                else {
                    $scope.institution = '#';
                }
            });
        };

        $scope.remaindersremarks = function (formvalid) {

            if (formvalid.$valid) {
                var data = {
                    "Save_Remainders_Remarks": $scope.Temp_notification
                };

                apiService.create("Login/SaveRemaindersRemarks", data).then(function (promise) {
                    swal("Remarks Captured");
                    $('#modalremainders').modal('hide');
                });
            } else {
                $scope.submittedremainders = true;
            }
        };

        $scope.interactedrmks = function () {
            return $scope.submittedremainders;
        };


        //=================================Interaction
        $scope.intercationclick = function () {
            $window.location.href = 'http://' + HostName + '/#/app/IVRM_Interactions/';

        };
    }]);
