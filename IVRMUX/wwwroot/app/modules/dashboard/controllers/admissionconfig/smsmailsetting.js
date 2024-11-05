
(function () {
    'use strict';
    angular
.module('app')
.controller('smsmailsettingController', smsmailsettingController)
    smsmailsettingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$compile', '$q', '$sce']
    function smsmailsettingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $compile, $q,  $sce,) {
        
        $scope.ISES_SMSActiveFlag = "On";
        $scope.ISES_MailActiveFlag = "On";
        $scope.ISES_SMSMessage = "";
        $scope.ISES_Mail_Message = "";
        $scope.isesid = "";
        $scope.ISES_PNActiveFlg = false;


        $scope.mobilesstd = [{ id: 'mobilestd1' }];
        $scope.addNewMobile1std = function () {
            var newItemNostd = $scope.mobilesstd.length + 1;
            if (newItemNostd <= 5) {
                $scope.mobilesstd.push({ 'id': 'mobilestd' + newItemNostd });
            }


            if (newItemNostd == 4) {
                $scope.myForm1.$setPristine();
            }

        };
        $scope.removeNewMobile1std = function (index, curval1std) {

            var newItemNostd2 = $scope.mobilesstd.length - 1;
            if (newItemNostd2 !== 0) {
                $scope.delmsrd = $scope.mobilesstd.splice(index, 1);
            }


        };

        $scope.onview = function (filepath, filename) {
            debugger;
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    //  $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var pdfId = document.getElementById("pdfId");
                    pdfId.removeChild(pdfId.childNodes[0]);
                    var embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '1000');
                    pdfId.appendChild(embed);


                    $('#showpdf').modal('show');
                });
        };



        $scope.removeNewEmail1std = function (index, id) {
            var newItemNostd = $scope.emailsstd.length - 1;
            if (newItemNostd !== 0) {
                $scope.delmsrd = $scope.emailsstd.splice(index, 1);
                
            }

        }
        $scope.showAddEmail1std = function (email) {
            return email.id === $scope.emailsstd[$scope.emailsstd.length - 1].id;
        };
        $scope.emailsstd = [{ id: 'emailsstd1' }];
        $scope.addNewEmail1std = function () {
            var newItemNostd2 = $scope.emailsstd.length + 1;
            if (newItemNostd2 <= 5) {
                $scope.emailsstd.push({ 'id': 'emailsstd' + newItemNostd2 });
            }
        };



        $scope.removeNewEmail1std2 = function (index, id) {
            var newItemNostd2 = $scope.emailsstd2.length - 1;
            if (newItemNostd2 !== 0) {
                $scope.delmsrd2 = $scope.emailsstd2.splice(index, 1);

            }

        }
        $scope.showAddEmail1std2 = function (email) {
            return email.id === $scope.emailsstd2[$scope.emailsstd2.length - 1].id;
        };
        $scope.emailsstd2 = [{ id: 'emailsstd2' }];
        $scope.addNewEmail1std2 = function () {
            var newItemNostd22 = $scope.emailsstd2.length + 1;
            if (newItemNostd22 <= 5) {
                $scope.emailsstd2.push({ 'id': 'emailsstd2' + newItemNostd22 });
            }
        };


        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }

        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }
        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        }
        $scope.IsHidden3 = true;
        $scope.ShowHide3 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden3 = $scope.IsHidden3 ? false : true;
        }
        $scope.IsHidden4 = true;
        $scope.ShowHide4 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden4 = $scope.IsHidden4 ? false : true;
        }

        $scope.searchValueUEM = '';
        $scope.IsHidden5 = true;
        $scope.IsHidden5 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden5 = $scope.IsHidden5 ? false : true;
        }
        $scope.urlimage = '';
        $scope.load = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("SMSEmailSetting/getalldetails", pageid).
            then(function (promise) {
                if (promise.count > 0)
                {
                    $scope.emailSmsSettingList = promise.emailSmsSettingList;
                }
                else {
                    swal("No Records Found.....!!");
                }
                $scope.institutionModuleList = promise.institutionModuleList;
                $scope.rolelist = promise.rolelist;
              
                $scope.institutionPageLists = promise.institutionPageList;
                //$scope.pageWiseHeaderList = promise.pageWiseHeaderList;
                $scope.emailtemplatelist = promise.emailtemplatelist;
            })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.getModulePage = function (module) {
            var moduleId = $scope.IVRMIM_Id;
            apiService.getURI("SMSEmailSetting/getmodulePage", moduleId).
           then(function (promise) {
               $scope.institutionPageLists = promise.institutionPageList;
               $scope.IVRMIMP_Id = $scope.institutionPageLists[0].ivrmimP_Id;
           })
        }

        $scope.fff = 'https://dcampusstrg.blob.core.windows.net/files/17/IVRSSOUNDTRACKS/cfafb0ef-8406-4d30-94c9-4e19ad1456a3.wav';
        $scope.UploadSoundfile1 = [];
        $scope.UploadSoundfile = function (input, document) {
            $scope.UploadSoundfile1 = input.files;
            if (input.files && input.files[0]) {
                if ((input.files[0].type == "audio/mp3" || input.files[0].type == "audio/wav") && input.files[0].size <= 922097152) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blah')
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadsoundfiel();
                }
                else if (input.files[0].type != "audio/mp3" || input.files[0].type != "audio/wav") {
                    swal("Please Upload the sound file");
                    return;
                } else if (input.files[0].size > 922097152) {
                    swal("sound size should be less than 922MB");
                    return;
                }
            }
        };
        function Uploadsoundfiel() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.UploadSoundfile1.length; i++) {
                formData.append("File", $scope.UploadSoundfile1[i]);
            }
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadsoundtrack", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    debugger;
                    defer.resolve(d);
                   // swal(d);
                    $scope.urlimage = d;
                    $scope.abcd = $scope.urlimage;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
           // Uploads1(miid);
        }


        $scope.getHeader = function () {
            var data = {
                "IVRMIMP_Id": $scope.IVRMIMP_Id,
                "IVRMIM_Id": $scope.IVRMIM_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("SMSEmailSetting/", data).
                  then(function (promise) {
                      $scope.pageWiseHeaderList = promise.pageWiseHeaderList;
                  })
        }

        $scope.htmldata1 = '';
        $scope.viewemailtempate = function (dd) {
            debugger;
            $scope.htmldata1 = dd;

            var e1 = angular.element(document.getElementById("test"));
            $compile(e1.html($scope.htmldata1))(($scope));
        }

        $scope.htmldata = '';
        $scope.viewtempate = function (user) {
            $scope.viewtempatedata = [];
            var data = {
                "ISMHTML_Id":user,
              
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("SMSEmailSetting/viewtempate/", data).
                  then(function (promise) {
                      $scope.viewtempatedata = promise.emailtemplatelist;
                      $scope.htmldata = $scope.viewtempatedata[0].ismhtmL_HTMLTemplate;

                      var e1 = angular.element(document.getElementById("test"));
                      $compile(e1.html($scope.htmldata))(($scope));


                  })
        }

        $scope.getParameter = function () {
            
            $scope.screen1 = true;
            var hearderId = $scope.ISES_Template_Name;
            apiService.getURI("SMSEmailSetting/getParameter", hearderId).
           then(function (promise) {
               
               $scope.data = promise.parameterList;
           });
        }

        $scope.SelectedFileForUploadz = [];
        $scope.templateFileUpload = function (input) {
            $scope.SelectedFileForUploadz = input.files;

        }
        $scope.roleidsss = [];
        $scope.saveSmsEmailSettings = function () {
            debugger;
            $scope.roleidsss = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var smsFlag = $scope.ISES_SMSActiveFlag;
                var emailFlag = $scope.ISES_MailActiveFlag;
                if (smsFlag == "On") {
                    smsFlag = true;
                }
                else {
                    smsFlag = false;
                }
                if (emailFlag == "On") {
                    emailFlag = true;
                }
                else {
                    emailFlag = false;
                }
                if ($scope.isesid != "" && $scope.isesid != null) {
                    $scope.ises = $scope.isesid;
                }
                //var formData = new FormData();
                //for (var i = 0; i <= $scope.selectFileforUploadzd.length; i++) {
                //    formData.append("File", $scope.SelectedFileForUploadzd[i]);
                //}
                
                //for (var j = 0; j < $scope.data.length; j++) {
                //    formData.append('templateParameter[' + j + '][ISMP_ID]', $scope.data[j].ismP_ID);
                //    formData.append('templateParameter[' + j + '][ISMP_NAME]', $scope.data[j].ismP_NAME);
                //}
                //formData.append("IVRMIM_Id", $scope.IVRMIM_Id);
                //formData.append("IVRMIMP_Id", $scope.IVRMIMP_Id);
                //formData.append("ISES_Template_Name", $scope.ISES_Template_Name);
                //formData.append("ISES_SMSMessage", $scope.ISES_SMSMessage);
                //formData.append("ISES_SMSActiveFlag", smsFlag);
                //formData.append("ISES_MailSubject", $scope.ISES_MailSubject);
                //formData.append("ISES_MailBody", $scope.ISES_MailBody);
                //formData.append("ISES_MailFooter", $scope.ISES_MailFooter);
                //formData.append("ISES_MailActiveFlag", emailFlag);
                //formData.append("ISES_Mail_Message", $scope.ISES_Mail_Message);
                //formData.append("ISES_Id", $scope.ises)
                var mobilesstd = [];
                angular.forEach($scope.mobilesstd, function (mobile) {
                    if (mobile.hrmemnO_MobileNo != undefined && mobile.hrmemnO_MobileNo != "") {
                        mobilesstd.push(mobile);
                    }
                });

                var emailsstd = [];
                angular.forEach($scope.emailsstd, function (email) {
                    if (email.hrmeM_EmailId != undefined && email.hrmeM_EmailId != "") {
                        emailsstd.push(email);
                    }
                });

                var emailsstd2 = [];
                angular.forEach($scope.emailsstd2, function (email) {
                    if (email.hrmeM_EmailId != undefined && email.hrmeM_EmailId != "") {
                        emailsstd2.push(email);
                    }
                });
                angular.forEach($scope.rolelist, function (ss) {
                    if (ss.empck === true) {
                        $scope.roleidsss.push(ss);
                    }
                });

                if ($scope.ISMHTML_Id == '' || $scope.ISMHTML_Id == null) {
                    $scope.ISMHTML_Id = undefined;
                }
                var data = {
                    "ISES_Id": $scope.ises,
                    "IVRMIM_Id": $scope.IVRMIM_Id,
                    "IVRMIMP_Id": $scope.IVRMIMP_Id,
                    "ISES_Template_Name": $scope.ISES_Template_Name,
                    "ISES_SMSMessage": $scope.ISES_SMSMessage,
                    "ISES_SMSActiveFlag": smsFlag,
                    "ISES_MailSubject": $scope.ISES_MailSubject,
                    "ISES_MailFooter": $scope.ISES_MailFooter,
                    "ISES_MailActiveFlag": emailFlag,
                    "ISES_EnableSMSCCFlg": $scope.ISES_EnableSMSCCFlg,
                    "ISES_AlertBeforeDays": $scope.ISES_AlertBeforeDays,
                    "ISMHTML_Id": $scope.ISMHTML_Id,
                    "ISES_EnableMailCCFlg": $scope.ISES_EnableMailCCFlg,
                    "ISES_EnableMailBCCFlg": $scope.ISES_EnableMailBCCFlg,
                    "ISES_PNActiveFlg": $scope.ISES_PNActiveFlg,
                    "ISES_PNMessage": $scope.ISES_PNMessage,
                    "ISES_IVRSTextMsg": $scope.ISES_IVRSTextMsg,
                    "ISES_IVRSVoiceFile": $scope.urlimage,
                    filelist: $scope.materaldocuupload,
                    mobile_list_dto: mobilesstd,
                    email_list_dto: emailsstd,
                    email_list_dtocc: emailsstd2,
                    roleids: $scope.roleidsss,
                    "ISES_TemplateId": $scope.ISES_TemplateId
            } 


                debugger;
                apiService.create("SMSEmailSetting/save/", data).
                    then(function (promise) {

                        if (promise.message=='dup') {
                            swal('Template Already Registered');
                        }
                        else {
                            if (promise.returnval == true) {
                                swal('Record Saved/Updated Successfully');
                            }
                            else {
                                swal('Error');
                            }
                        }
                        $state.reload();

                    })
            };
        }



        $scope.materaldocuupload = [{ id: 'Materal1' }];

        $scope.addmateral = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
            }
        };

        $scope.removemateral = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

            if ($scope.materaldocuupload.length === 0) {
                //data
            }
        };
        // Save Function For Materal Guide Upload

        $scope.uploadmateraldocuments1 = [];

        $scope.uploadmateraldocuments = function (input, document) {

            $scope.uploadmateraldocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload MP4, Pdf, Image Files Only");
                }
            }
        };

        function UploaddianmateralPhoto(data) {
            console.log("Student Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadmateraldocuments1.length; i++) {
                formData.append("File", $scope.uploadmateraldocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/emailfileattachupload", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.cfilepath = d;
                    data.cfilename = $scope.filename;
                    $('#').attr('src', data.cfilepath);
                    var img = data.cfilepath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.cfilepath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };


        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.cfilepath;
            $scope.videdfd = data.cfilepath;
            $scope.movie = { src: data.cfilepath };
            $scope.movie1 = { src: data.cfilepath };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.cfilepath });
            console.log($scope.view_videos);
        };

        $scope.showpdf = false;
        $scope.downloadview = function (pdfview) {
            $scope.pdfurl = pdfview;
            $scope.showpdf = true;
            $('#showpdf').modal('show');
        };

        $scope.backtoview = function () {
            $scope.showpdf = false;
        };



        //$scope.SelectedFileForUploadzd = [];

        //$scope.selectFileforUploadzd = function (input, document) {
            
        //    if (input.files[0].type != "image/png" && input.files[0].type != "image/gif" &&
        //        input.files[0].type != "image/jpeg" && input.files[0].type != "application/pdf" &&
        //        input.files[0].type != "application/msword" &&
        //        input.files[0].type != "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        //        && input.files[0].type != "application/vnd.ms-excel"
        //        && input.files[0].type != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
        //        swal("Unsupported File Format.Allowed only .png,.gif,.jpeg,.pdf,.doc/docx,.xls/xlsx");
        //        angular.element("input[type='file']").val(null);
        //        return;
        //    }
        //    $scope.SelectedFileForUploadzd = input.files;
        //    if (input.files && input.files[0]) {

        //        if (input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
        //        {
        //            var reader = new FileReader();

        //            //reader.onload = function (e) {

        //            //    $('#' + document.amsmD_Id)
        //            //        .attr('src', e.target.result)
        //            //};
        //            //reader.readAsDataURL(input.files[0]);
        //            //Uploadprofiled(document);

        //        }
        //            //else if (input.files[0].type != "text/html") {
        //            //    swal("Please Upload the .html file");
        //            //    angular.element("input[type='file']").val(null);
        //            //    return;
        //            //} 
        //        else if (input.files[0].size > 2097152) {
        //            swal("File size should be less than 2MB");
        //            return;
        //        }

        //    }
        //}

        $scope.clearid = function () {
            $state.reload();
            //$scope.IVRMIM_Id = "";
            //$scope.IVRMIMP_Id = "";
            //$scope.ISES_Template_Name = "";
            //$scope.ISES_SMSMessage = "";
            //$scope.ISES_MailSubject = "";
            //$scope.ISES_MailBody = "";
            //$scope.ISES_MailFooter = "";
            //$scope.ISES_Mail_Message = "";
            //$scope.ISES_SMSActiveFlag = "";
            //$scope.ISES_MailActiveFlag = "";

        }

        $scope.Delete = function (emailId, SweetAlert) {
            $scope.SmsEmailId = emailId.iseS_Id;
            var id = $scope.SmsEmailId
            swal({
                title: "Are you sure?",
                text: "Do you want to delete record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("SMSEmailSetting/deletedetails", id).
                    then(function (promise) {
                        $scope.emailSmsSettingList = promise.emailSmsSettingList;
                        if (promise.returnval == true)
                        {
                            swal('Record Deleted Successfully!', 'success');
                        }
                        else {
                            swal('Failed to Delete Record!', 'Failed');
                        }
                       

                    });
                }
                else {
                    swal("Record Deletion Cancelled", "Failed");
                }
            });
        }

        $scope.EditDetails = function (smsEmail) {
            $scope.urlimage = '';
            $('#blah').attr('src', $scope.urlimage);
            $scope.disble = true;
            $scope.screen1 = true;
            $scope.isesid = smsEmail.iseS_Id;
            var emailId = $scope.isesid;
            //orgid = 12;
            apiService.getURI("SMSEmailSetting/getdetails", emailId).
            then(function (promise) {
                
                $scope.pageWiseHeaderList = promise.pageWiseHeaderList;

                debugger;
                if (promise.ivrmhE_Id > 0)
                {
                    $scope.ISES_Template_Name =promise.ivrmhE_Id;
                }

                $scope.ISMHTML_Id = promise.ismhtmL_Id

                $scope.ises = promise.emailSmsSettingList[0].iseS_Id;
                $scope.IVRMIM_Id = promise.emailSmsSettingList[0].ivrmiM_Id;
                $scope.IVRMIMP_Id = promise.emailSmsSettingList[0].ivrmimP_Id;
                $scope.ISES_SMSMessage = promise.emailSmsSettingList[0].iseS_SMSMessage;
                $scope.ISES_MailSubject = promise.emailSmsSettingList[0].iseS_MailSubject;
                $scope.ISES_MailBody = promise.emailSmsSettingList[0].iseS_MailBody;
                $scope.ISES_MailFooter = promise.emailSmsSettingList[0].iseS_MailFooter;
                $scope.ISES_Mail_Message = promise.emailSmsSettingList[0].iseS_Mail_Message;
                $scope.ISES_MailHTMLTemplate = promise.emailSmsSettingList[0].iseS_MailHTMLTemplate;
                $scope.ISES_EnableSMSCCFlg = promise.emailSmsSettingList[0].iseS_EnableSMSCCFlg;
                $scope.ISES_EnableMailCCFlg = promise.emailSmsSettingList[0].iseS_EnableMailCCFlg;
                $scope.ISES_EnableMailBCCFlg = promise.emailSmsSettingList[0].iseS_EnableMailBCCFlg;

                $scope.ISES_AlertBeforeDays = promise.emailSmsSettingList[0].iseS_AlertBeforeDays;
                $scope.ISES_IVRSTextMsg = promise.emailSmsSettingList[0].iseS_IVRSTextMsg;
                $scope.ISES_PNMessage = promise.emailSmsSettingList[0].iseS_PNMessage;

                $scope.ISES_TemplateId = promise.emailSmsSettingList[0].iseS_TemplateId;

                if (promise.emailSmsSettingList[0].iseS_PNActiveFlg === null) {
                    $scope.ISES_PNActiveFlg = false;
                }
                else {
                    $scope.ISES_PNActiveFlg = promise.emailSmsSettingList[0].iseS_PNActiveFlg;
                }

                if (promise.emailSmsSettingList[0].iseS_IVRSVoiceFile !== null && promise.emailSmsSettingList[0].iseS_IVRSVoiceFile !== '') {
                    $scope.urlimage = promise.emailSmsSettingList[0].iseS_IVRSVoiceFile;
                    $('#blah').attr('src', $scope.urlimage);
                }



                if (promise.emailSmsSettingList[0].iseS_SMSActiveFlag == true) {
                    $scope.ISES_SMSActiveFlag = "On";
                }
                else {
                    $scope.ISES_SMSActiveFlag = "Off";
                }
                if (promise.emailSmsSettingList[0].iseS_MailActiveFlag == true) {
                    $scope.ISES_MailActiveFlag = "On";
                }
                else {
                    $scope.ISES_MailActiveFlag = "Off";
                }
                $scope.data = promise.parameterList;

                $scope.moblist = promise.mobilenolist;
                $scope.emblist = promise.emailistmcc;
                $scope.emblistbcc = promise.emailistbcc;

                $scope.mobilesstd1 = [];
                angular.forEach($scope.moblist, function (ee) {
                    $scope.mobilesstd1.push({ hrmemnO_MobileNo: ee })

                })
                if ($scope.mobilesstd1.length > 0) {
                    $scope.mobilesstd = $scope.mobilesstd1;
                }



                $scope.emailsstd1 = [];
                angular.forEach($scope.emblist, function (ee) {
                    $scope.emailsstd1.push({ hrmeM_EmailId: ee })

                })
                if ($scope.emailsstd1.length > 0) {
                    $scope.emailsstd = $scope.emailsstd1;
                }

                $scope.emailsstd1b = [];
                angular.forEach($scope.emblistbcc, function (ee) {
                    $scope.emailsstd1b.push({ hrmeM_EmailId: ee })

                })
                if ($scope.emailsstd1b.length > 0) {
                    $scope.emailsstd2 = $scope.emailsstd1b;
                }

                angular.forEach($scope.rolelist, function (ss) {
                    angular.forEach(promise.rolelist, function (ff) {
                        if (ff.ivrmrT_Id === ss.ivrmrT_Id) {
                            ss.empck = true;
                        }

                    })

                })



                $scope.materaldocuupload = [{ id: 'Materal1' }];
                if (promise.editfiles != null && promise.editfiles.length > 0) {
                    $scope.materaldocuupload = promise.editfiles;
                    angular.forEach($scope.materaldocuupload, function (ddd) {

                        var img = ddd.cfilepath;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        ddd.filetype = lastelement;
                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                            ddd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + ddd.cfilepath;
                        }
                    })
                }

            })
        }

        //sms active deactive form

        $scope.activedeactivesms = function (data, SweetAlert) {
            var mgs = "";
            if (data.iseS_SMSActiveFlag == false) {
                mgs = "Activate";
            }
            else {
                mgs = "De-Activate";
            }
            swal({
                title: "Are You Sure?",
                text: "Do You Want to " + mgs + " SMS Settings ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.create("SMSEmailSetting/activedeactivesms", data).
                    then(function (promise) {

                        if (promise.message != null) {
                            swal(promise.message);
                        }
                        else {
                            if (promise.returnval === true) {
                                swal('SMS Settings ' + mgs + ' Successfully');
                            }
                            else {
                                swal('Failed To ' + mgs + 'SMS Settings Record ');
                            }
                        }

                        $state.reload();
                    })
                }
                else {
                    swal("Cancelled");
                }
            });
        }

        //Email Active  deactive
        $scope.activedeactiveemail = function (data, SweetAlert) {
            var mgs = "";
            if (data.iseS_MailActiveFlag == false) {
                mgs = "Activate";
            }
            else {
                mgs = "De-Activate";
            }
            swal({
                title: "Are You Sure?",
                text: "Do You Want to " + mgs + " Email Settings ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.create("SMSEmailSetting/activedeactiveemail", data).
                    then(function (promise) {

                        if (promise.message != null) {
                            swal(promise.message);
                        }
                        else {
                            if (promise.returnval === true) {
                                swal('Email Settings ' + mgs + ' Successfully');
                            }
                            else {
                                swal('Email To ' + mgs + 'SMS Settings Record ');
                            }
                        }

                        $state.reload();
                    })
                }
                else {
                    swal("Cancelled");
                }
            });
        }

    }

    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });

    angular
    .module('app').directive('txtArea', function () {
        return {
            restrict: 'AE',
            replace: 'true',
            scope: { data: '=', model: '=ngModel' },
            template: "<textarea></textarea>",
            link: function (scope, elem) {
                scope.$watch('data', function (newVal) {
                    if (newVal) {
                        debugger;

                        if (scope.model === undefined || scope.model === null) {
                            scope.model = '';
                        }
                        scope.model += newVal[0];
                    }
                });
            }
        };
    });
})();