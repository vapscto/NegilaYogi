
(function () {
    'use strict';
    angular
        .module('app')
        .controller('StudentGatePassController', StudentGatePassController)

    StudentGatePassController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache', '$filter', '$compile']
    function StudentGatePassController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache, $filter, $compile) {

        //$scope.student_printoption = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.searchValue23 = "";
        $scope.mindate = new Date();
        $scope.maxdate = new Date();

        $scope.details = false;
        $scope.otpsucess = false;

        $scope.sortReverse = true;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        //$scope.gphS_DateTime = new Date();
        //========================TO  GEt The Values iN Grid
        $scope.BindData = function () {
            //var pageid = 2;
            apiService.getURI("StudentGatePass/getdetails", 2).then(function (promise) {
                $scope.yearlist = promise.yearlist;
                $scope.alldata = promise.alldata;
            })
        };


        //========================================Get Class Data
        $scope.get_class = function () {
            $scope.details = false;
            $scope.classList = [];
            $scope.sectionDropdown = [];
            $scope.studentList = [];
            $scope.asmS_Id = "";
            $scope.asmcL_Id = "";
            $scope.amsT_Id = "";
            $scope.otpsucess = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("StudentGatePass/get_class", data).then(function (promise) {
                if (promise.classList.length > 0) {
                    $scope.classList = promise.classList;
                }
                else {
                    swal('Record Not Found!');
                }

            })
        }


        //========================================Get Section Data
        $scope.get_section = function () {
            $scope.details = false;
            $scope.sectionDropdown = [];
            $scope.asmS_Id = "";
            $scope.studentList = [];
            $scope.amsT_Id = "";
            $scope.otpsucess = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            }
            apiService.create("StudentGatePass/get_section", data).then(function (promise) {
                if (promise.sectionList.length > 0) {
                    $scope.sectionDropdown = promise.sectionList;
                }
                else {
                    swal('Record Not Found!');
                }
            })
        }


        //========================================Get Student Data
        $scope.get_student = function () {
            $scope.details = false;
            $scope.studentList = [];
            $scope.amsT_Id = "";
            $scope.otpsucess = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id
            }
            apiService.create("StudentGatePass/get_student", data).then(function (promise) {
                if (promise.studentList.length > 0) {
                    $scope.studentList = promise.studentList;
                }
                else {
                    swal('Record Not Found!');
                }
            })
        }

        //========================================Get Student Data
        $scope.getstudentdetails = function () {
            $scope.otpsucess = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id,
                "AMST_Id": $scope.amsT_Id,
                "GPHS_DateTime": $scope.gphS_DateTime
            }
            apiService.create("StudentGatePass/GetStudDetails", data).then(function (promise) {

                if (promise.detailsforstudent !== null && promise.detailsforstudent.length > 0) {
                    if (promise.message === null || promise.message === "") {
                        $scope.details = true;
                        $scope.otpsucess = false;
                        $scope.detailsforstudent = promise.detailsforstudent;

                        $scope.fathername = $scope.detailsforstudent[0].fathername;
                        $scope.AMST_FatherMobleNo = $scope.detailsforstudent[0].AMST_FatherMobleNo;
                        $scope.AMST_FatheremailId = $scope.detailsforstudent[0].AMST_FatheremailId;
                        $scope.ANST_FatherPhoto = $scope.detailsforstudent[0].ANST_FatherPhoto;

                        $scope.mothername = $scope.detailsforstudent[0].mothername;
                        $scope.AMST_MotherMobileNo = $scope.detailsforstudent[0].AMST_MotherMobileNo;
                        $scope.ANST_MotherPhoto = $scope.detailsforstudent[0].ANST_MotherPhoto;
                        $scope.AMST_MotherEmailId = $scope.detailsforstudent[0].AMST_MotherEmailId;

                        $scope.address = $scope.detailsforstudent[0].address;

                        $scope.studentname = $scope.detailsforstudent[0].studentname;
                        $scope.admno = $scope.detailsforstudent[0].admno;
                        $scope.rollno = $scope.detailsforstudent[0].rollno;
                        $scope.classname = $scope.detailsforstudent[0].classname;
                        $scope.sectionname = $scope.detailsforstudent[0].sectionname;
                        $scope.studentphotoname = $scope.detailsforstudent[0].studentphotoname;
                        $scope.yearname = $scope.detailsforstudent[0].yearname;
                        $scope.mobileno = $scope.detailsforstudent[0].mobileno;
                        $scope.emailid = $scope.detailsforstudent[0].emailid;

                        $scope.gphS_IDCardNo = $scope.detailsforstudent[0].admno;
                    }
                    else {
                        swal(promise.message);
                    }
                }
                else {
                    swal('Record Not Found!');
                }

            })
        }

        // ***************** GET OTP ****************** //
        $scope.getotp = function () {
            var remarks = "";
            if ($scope.gphS_Remarks !== undefined && $scope.gphS_Remarks !== null) {
                remarks = $scope.gphS_Remarks;
            } else {
                remarks = "";
            }
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id,
                "AMST_Id": $scope.amsT_Id,
                "GPHS_Remarks": remarks
            }

            apiService.create("StudentGatePass/getotp", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.message !== null && promise.message !== "") {
                        swal(promise.message);
                        $('#otpverification').modal('show');
                    }
                }

            });
        };

        /* Resend OTP */
        $scope.resendnewotp = function () {
            var remarks = "";
            if ($scope.gphS_Remarks !== undefined && $scope.gphS_Remarks !== null) {
                remarks = $scope.gphS_Remarks;
            } else {
                remarks = "";
            }
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id,
                "AMST_Id": $scope.amsT_Id,
                "GPHS_Remarks": remarks
            }

            apiService.create("StudentGatePass/getotp", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.message !== null && promise.message !== "") {
                        swal(promise.message);
                    }
                }

            });
        };


        //============================================== TO Save The Data        
        $scope.submitted = false;
        $scope.obj = {};
        $scope.savedata = function (obj) {

            if ($scope.myForm.$valid) {
                var data = {
                    "GPHS_Id": $scope.gphS_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "GPHS_IDCardNo": $scope.gphS_IDCardNo,
                    "GPHS_Remarks": $scope.gphS_Remarks,
                    "GPHS_DateTime": new Date($scope.gphS_DateTime).toDateString(),
                    "AMST_Id": $scope.amsT_Id,
                    "GPHS_ReceiverName": obj.receiverName,
                    "GPHS_ReceiverPhoneNo": obj.GPHS_ReceiverPhoneNo,
                    "GPHS_ReceiverIdProof": obj.GPHS_ReceiverIdProof,
                    "GPHS_ReceiverIdProofNo": obj.GPHS_ReceiverIdProofNo,
                    "newotpsave": $scope.newotpsave,

                }
                apiService.create("StudentGatePass/saveRecord", data).then(function (promise) {
                    if (promise.returnval != null && promise.dulicate != null) {
                        if (promise.dulicate == false) {
                            if (promise.returnval == true) {
                                if ($scope.gphS_Id > 0) {
                                    swal('Record Updated Successfully!!!');
                                    $state.reload();
                                }
                                else {
                                    swal('Record Saved Successfully!!!');

                                    ////$scope.student_printoption = false;
                                    //$scope.SchoolLogo = promise.institution[0].mI_Logo;
                                    //$scope.SchollName = promise.institution[0].mI_Name;
                                    //$scope.SchollAdd = promise.institution[0].mI_Address1;
                                    ////$scope.student_gate_pass = true;
                                    //$scope.currentstuddata = promise.currentstuddata;
                                    //$scope.alldata.length = 0;

                                    $state.reload();
                                }
                            }
                            else {
                                if (promise.returnval == false) {
                                    if ($scope.gphS_Id > 0) {
                                        swal('Record Not Update Successfully!!!');
                                        $state.reload();
                                    }
                                    else {
                                        swal('Record Not Saved Successfully!!!');
                                        $state.reload();
                                    }
                                }
                            }
                        }
                        else {
                            swal("Record already exist");
                        }
                        // $state.reload();
                    }
                    else {
                        swal("Kindly Contact Administrator!!!");
                    }

                })
            }
            else {
                $scope.submitted = true;
            }
        };

        //=============================================Get Edit Data
        $scope.EditRecord = function (EditRecord) {

            var data = {
                "GPHS_Id": EditRecord.gphS_Id,
            }
            apiService.create("StudentGatePass/EditRecord/", data).then(function (promise) {

                if (promise.editlist.length > 0) {

                    $scope.gphS_Id = promise.editlist[0].gphS_Id;
                    $scope.gphS_DateTime = new Date(promise.editlist[0].gphS_DateTime);
                    $scope.gphS_IDCardNo = promise.editlist[0].gphS_IDCardNo;
                    $scope.gphS_Remarks = promise.editlist[0].gphS_Remarks;

                    $scope.spccsH_Remarks = promise.editlist[0].spccsH_Remarks;
                    $scope.obj.receiverName = promise.editlist[0].gphS_ReceiverName;
                    $scope.obj.GPHS_ReceiverPhoneNo = promise.editlist[0].gphS_ReceiverPhoneNo;
                    $scope.obj.GPHS_ReceiverIdProof = promise.editlist[0].gphS_ReceiverIdProof;
                    $scope.obj.GPHS_ReceiverIdProofNo = promise.editlist[0].gphS_ReceiverIdProofNo;


                    $scope.asmaY_Id = promise.editlist[0].asmaY_Id;
                    $scope.get_class();
                     
                    $scope.asmcL_Id = promise.editlist[0].asmcL_Id;

                    $scope.get_section();
                    $scope.asmS_Id = promise.editlist[0].asmS_Id;

                    $scope.get_student();
                    $scope.amsT_Id = promise.editlist[0].amsT_Id;

                }
                else {
                    swal('No Record Found!');
                }
            })
        };

        //=========================================Form1
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //==============================================Active And Deactivate Row data
        $scope.deactivate = function (newuser1, SweetAlertt) {
            debugger;
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            var mgs = "";
            if (newuser1.gphS_ActiveFlg == false) {

                mgs = "Activate";

            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("StudentGatePass/deactive", newuser1).
                            then(function (promise) {

                                if (promise.returnval == true) {
                                    swal("Record" + mgs + "d Successfully!!");

                                }
                                else {
                                    swal("Record Not " + mgs + "d Successfully!!!");
                                }
                                $state.reload();

                            })
                    } else {
                        swal("Record " + mgs + " Cancelled");
                    }
                })
        }

        //===========================cancel
        $scope.cancel = function () {
            $state.reload();
        }

        //====================Print Option
        $scope.Print = function () {
            var innerContents = '';
            //innerContents = document.getElementById("test").innerHTML;
            if ($scope.FormatType == 'Format2') {
                innerContents = document.getElementById("test2").innerHTML;
            }
            else {
                innerContents = document.getElementById("test").innerHTML;
            }
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/Visitor_Management/InwardReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        }

        $scope.searchValue23 = "";



        //===================================View For OTP Verification

        $scope.checkstudent = function (user) {
            debugger;
            apiService.create("StudentGatePass/checkstudentdata", user).then(function (promise) {
                swal(promise);

                $scope.singlestudentdata = promise.singlestudentdata;

                $scope.gphS_Id = $scope.singlestudentdata[0].gphS_Id;

            })

        }

        //======================Form2 
        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };



        //========================================Get Verification
        $scope.submitted2 = false;
        $scope.get_otpverification = function (user) {
            if ($scope.myForm2.$valid) {
                debugger;
                var data = {
                    "GPHS_OTP": $scope.GPHS_OTP,
                    "GPHS_Id": user,
                }

                apiService.create("StudentGatePass/get_otpverification", data).then(function (promise) {

                    if (promise.returnval == true) {
                        swal('Your OTP Verification Is Success');
                        $state.reload();
                    }
                    else/* if (promise.message == "Please Register Your Mobile Number") */ {
                        swal(promise.message);
                        $scope.GPHS_OTP = "";
                    }
                    //else if (promise.message == "Please Enter Valid OTP Number") {
                    //    swal(promise.message);
                    //}
                    //else if (promise.message = "Your Time is Over. Again Resend Your OTP") {
                    //    swal(promise.message);
                    //    $scope.GPHS_OTP = "";
                    //}



                })
            }
            else {
                $scope.submitted2 = true;
            }
        }

        $scope.submitted2 = false;
        $scope.getnewotpverification = function (user) {
            if ($scope.myForm2.$valid) {
                $scope.newotpsave = $scope.GPHS_OTP;
                var data = {
                    "GPHS_OTP": $scope.GPHS_OTP,
                }

                apiService.create("StudentGatePass/getnewotpverification", data).then(function (promise) {

                    if (promise.returnval == true) {
                        swal('Your OTP Verification Is Success');
                        $state.reload();
                    }
                    else {
                        if (promise.message === "Success") {
                            $scope.otpsucess = true;
                            swal(promise.message);
                            $('#otpverification').modal('hide');
                        } else {
                            swal(promise.message);
                            $scope.GPHS_OTP = "";
                        }
                    }
                })
            }
            else {
                $scope.submitted2 = true;
            }
        }


        //==================================Resend OTP
        $scope.Veributton2 = false;
        $scope.Veributton1 = true;
        $scope.resendotp = function (user) {

            $scope.Veributton2 = true;
            $scope.Veributton1 = false;
            $scope.GPHS_OTP = "";

            var data = {
                "GPHS_Id": user,
            }

            apiService.create("StudentGatePass/resendotp", data).then(function (promise) {
                $scope.gphS_Id = promise.otpid[0];

            })
        }

        //==================== After Resend OTP Verification
        $scope.get_otpverification22 = function (user) {
            if ($scope.myForm2.$valid) {
                debugger;
                var data = {
                    "GPHS_OTP": $scope.GPHS_OTP,
                    "GPHS_Id": user,
                }

                apiService.create("StudentGatePass/get_otpverification22", data).then(function (promise) {

                    if (promise.returnval != false) {
                        swal('Your OTP Verification Is Success');
                        $state.reload();
                    }
                    else /*if (promise.message == "Please Enter Valid OTP Number")*/ {
                        swal(promise.message);
                    }
                    //else if (promise.message == "Please Register Your Mobile Number") {
                    //    swal(promise.message);
                    //}
                    //else if (promise.message =="Your Time is Over. Again Resend Your OTP") {
                    //    swal(promise.message);
                    //}
                    //else {
                    //    swal('Your OTP Verification Is Failed');
                    //}

                })
            }
            else {
                $scope.submitted2 = true;
            }
        }


        //============================= Print Data
        $scope.printbutton = function (user) {

            var data = {
                "GPHS_Id": user.gphS_Id,
                "ASMAY_Id": user.asmaY_Id,
            }

            apiService.create("StudentGatePass/printbutton", data).then(function (promise) {
                $scope.currentstuddata = promise.currentstuddata;
                $scope.amsT_Photoname = promise.currentstuddata[0].amsT_Photoname;
                $scope.SchoolLogo = promise.institution[0].mI_Logo;
                $scope.SchollName = promise.institution[0].mI_Name;
                $scope.SchollAdd = promise.institution[0].mI_Address1;
                $scope.MI_Id = promise.institution[0].mI_Id;
                $scope.htmldata = promise.htmldata;

                $scope.gphS_Id = $scope.currentstuddata[0].gphS_Id;
                $scope.studentname = $scope.currentstuddata[0].studentname;
                $scope.amsT_AdmNo = $scope.currentstuddata[0].amsT_AdmNo;
                $scope.asmcL_ClassName = $scope.currentstuddata[0].asmcL_ClassName;
                $scope.asmC_SectionName = $scope.currentstuddata[0].asmC_SectionName;
                $scope.amsT_MobileNo = $scope.currentstuddata[0].amsT_MobileNo;
                $scope.gphS_GatePassNo = $scope.currentstuddata[0].gphS_GatePassNo;
                $scope.gphS_IDCardNo_New = $scope.currentstuddata[0].gphS_IDCardNo;
                $scope.gphS_DateTime = new Date($scope.currentstuddata[0].gphS_DateTime);
                $scope.gphS_Remarks_New = $scope.currentstuddata[0].gphS_Remarks;
                $scope.trmR_RouteNo = $scope.currentstuddata[0].trmR_RouteNo;

                $scope.today = $filter('date')(new Date($scope.gphS_DateTime), 'yyyy-MM-dd HH:mm:ss');

                if (promise.htmldata != null && promise.htmldata != "" && promise.htmldata != undefined) {
                    var e1 = angular.element(document.getElementById("test"));
                    $compile(e1.html(promise.htmldata))(($scope));
                }
                $('#myModal1').modal('show');
            });
        }
    }
})();


