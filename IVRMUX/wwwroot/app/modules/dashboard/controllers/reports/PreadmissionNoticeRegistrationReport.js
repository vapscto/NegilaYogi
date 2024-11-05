(function () {
    'use strict';
    angular
        .module('app')
        .controller('PreadmissionNoticeRegistrationReportController', PreadmissionNoticeRegistrationReportController)

    PreadmissionNoticeRegistrationReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function PreadmissionNoticeRegistrationReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.submitted = false;
        $scope.search = "";

        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        //var paginationformasters;
        //var copty;
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        //    copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        //}
        //$scope.coptyright = copty;
        $scope.sortKey = "User_Name";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;

        $scope.ddate = {};
        $scope.ddate = new Date();

        //$scope.usrname = localStorage.getItem('username');
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        //if (admfigsettings.length > 0) {
        //    var logopath = admfigsettings[0].asC_Logo_Path;
        //}

        //$scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.canceldata = function () {
            $scope.PASRAN_NAME = '';
            $scope.PASRAN_FEEAMOUNT = '';
            $scope.PASRAN_PAYDATE = '';
            $scope.PASRAN_PAYTIME = '';
            $scope.ASMAY_Id = '';
            $scope.pasr_id = '';
            $scope.PASR_FirstName = '';
            $scope.PASR_DOB = '';
            $scope.PASR_Age = '';
            $scope.PASR_PerCity = '';
            $scope.IMCC_CategoryName = '';
            $scope.PASR_PerCountry = '';
            $scope.PASR_PerState = '';
            $scope.PASRANS_REMARKS = '';
            $scope.PASRANS_ADMDATE = '';
            $scope.PASRANS_TIME = '';
            $scope.yearlist = [];
            $scope.prospectuslist = [];
            $scope.arrlist = [];
            $scope.arrStatelist2 = [];
            $state.reload();
        }

        $scope.loaddata = function () {

            var pageid = 2;
            apiService.getURI("PreadmissionNoticeRegistrationReport/get_intial_data", pageid).
                then(function (promise) {

                    $scope.yearlist = promise.yearlist;
                    $scope.alldata1 = promise.alldata1;
                })
        }
        //Onchange Of Academic Year
        $scope.onchangeno = function () {
           
            $scope.pasr_id = '';
            $scope.PASR_FirstName = '';
            $scope.PASR_DOB = '';
            $scope.PASR_Age = '';
            $scope.PASR_PerCity = '';
            $scope.IMCC_CategoryName = '';
            $scope.PASR_PerCountry = '';
            $scope.PASR_PerState = '';
            $scope.PASRANS_REMARKS = '';
            $scope.PASRANS_ADMDATE = '';
            $scope.PASRANS_TIME = '';
           
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("PreadmissionNoticeRegistrationReport/getprospectusno", data).
                then(function (promise) {
                    if (promise.prospectuslist.length > 0) {
                        $scope.prospectuslist = promise.prospectuslist;
                        $scope.year_name = promise.prospectuslist[0].asmaY_Year;
                    }
                    else {
                        swal("Data not Available");
                    }
                    
                })
        }
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        //Onchange Of Prospectus Number
        $scope.onchangestudent = function () {
           
            var data = {
                "PASR_RegistrationNo": $scope.PASR_RegistrationNo,
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("PreadmissionNoticeRegistrationReport/getstudentlist", data).
                then(function (promise) {
                    $scope.PASR_FirstName = promise.studentDetails[0].pasR_FirstName;
                    $scope.PASR_DOB = promise.studentDetails[0].pasR_DOB;
                    $scope.PASR_Age = promise.studentDetails[0].pasR_Age;
                    $scope.PASR_PerCity = promise.studentDetails[0].pasR_PerCity;
                    $scope.CasteCategory_Id = promise.studentDetails[0].casteCategory_Id;
                    $scope.IMCC_CategoryName = promise.studentDetails[0].imcC_CategoryName;
                    $scope.arrlist = promise.countryDrpDown;
                    $scope.PASR_PerCountry = promise.studentDetails[0].ivrmmC_Id;
                    $scope.arrStatelist2 = promise.statedropdown;
                    $scope.PASR_PerState = promise.studentDetails[0].ivrmmS_Id;
                  
                })
        }
        $scope.clear = function () {
            $scope.ASMAY_Id = '';
            $scope.pasr_id = '';
            $scope.PASR_RegistrationNo ='';
            $scope.PASR_FirstName = '';
            $scope.PASR_DOB = '';
            $scope.PASR_Age = '';
            $scope.PASR_PerCity = '';
            $scope.IMCC_CategoryName = '';
            $scope.PASR_PerCountry = '';
            $scope.PASR_PerState = '';
            $scope.PASRANS_REMARKS = '';
            $scope.PASRANS_ADMDATE = '';
            $scope.PASRANS_TIME = '';

        }

        $scope.reportdetails = [];
        $scope.add = function () {
           
            if ($scope.ASMAY_Id != null /*&& $scope.pasr_id != null*/ && $scope.PASR_FirstName != null && $scope.PASR_RegistrationNo != null && $scope.PASR_DOB != null && $scope.PASR_Age != null && $scope.PASR_PerCity != null && $scope.CasteCategory_Id != null && $scope.PASRANS_REMARKS != null && $scope.PASRANS_ADMDATE != null && $scope.PASRANS_TIME != null) {

                if ($scope.pasr_id > 0) {
                    for (var i = $scope.reportdetails.length - 1; i >= 0; i--) {
                        if ($scope.reportdetails[i].pasr_id == $scope.pasr_id) {
                            $scope.reportdetails.splice(i, 1);
                        }
                    }
                    var a = $scope.reportdetails.length;
                    $scope.reportdetails.push({ sid: a + 1, /*pasr_id: $scope.pasr_id,*/ asmaY_Year: $scope.year_name, pasR_RegistrationNo: $scope.PASR_RegistrationNo, pasR_FirstName: $scope.PASR_FirstName, pasR_DOB: $scope.PASR_DOB, pasR_Age: $scope.PASR_Age, pasR_PerCity: $scope.PASR_PerCity, imcC_CategoryName: $scope.IMCC_CategoryName, pasranS_REMARKS: $scope.PASRANS_REMARKS, pasranS_ADMDATE: $scope.PASRANS_ADMDATE, pasranS_TIME: $scope.PASRANS_TIME });
                    console.log($scope.reportdetails);
                    $scope.details = true;
                    $scope.clear();
                }
                else {
                    var a = $scope.reportdetails.length;
                    $scope.reportdetails.push({ sid: a + 1, /*pasr_id: $scope.pasr_id,*/ asmaY_Year: $scope.year_name, pasR_RegistrationNo: $scope.PASR_RegistrationNo, pasR_FirstName: $scope.PASR_FirstName, pasR_DOB: $scope.PASR_DOB, pasR_Age: $scope.PASR_Age, pasR_PerCity: $scope.PASR_PerCity, imcC_CategoryName: $scope.IMCC_CategoryName, pasranS_REMARKS: $scope.PASRANS_REMARKS, pasranS_ADMDATE: $scope.PASRANS_ADMDATE, pasranS_TIME: $scope.PASRANS_TIME });
                    console.log($scope.reportdetails);
                    $scope.details = true;
                    $scope.clear();
                }
            }
            else {
                swal("Add Student Details");
            }
        };
        $scope.remove = function (user) {
            if ($scope.editEmployee > 0) {              
                if (user.pasr_id > 0) {
                    var data = {
                        "pasr_id": user.pasr_id
                    };
                    swal({
                        title: "Are you sure?",
                        text: "Do You Want To delete the Record?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: false,
                        closeOnCancel: false
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                apiService.create("PreadmissionNoticeRegistrationReport/remove", data).then(function (promise) {
                                    if (promise.returnval == 'true') {
                                        swal("Record Deleted Successfully...");
                                        for (var i = $scope.reportdetails.length - 1; i >= 0; i--) {
                                            if ($scope.reportdetails[i].sid == user.sid) {
                                                $scope.reportdetails.splice(i, 1);
                                            }
                                        }
                                    }
                                });
                                if ($scope.reportdetails.length == 0) {
                                    $scope.details = false;
                                }
                            }
                            else {
                                swal("Record Deletion Cancelled");
                            }
                        });
                }
            }
            else {
                for (var i = $scope.reportdetails.length - 1; i >= 0; i--) {
                    if ($scope.reportdetails[i].sid == user.sid) {
                        $scope.reportdetails.splice(i, 1);
                    }
                }
            }
        };
        $scope.reportdetailsnew = [];
        $scope.Savedata = function () {
            debugger;
            $scope.reportdetailsnew = [];
            if ($scope.reportdetails.length > 0) {
                $scope.reportdetailsnew = $scope.reportdetails;
            }    
            $scope.PASRAN_PAYDATE = new Date($scope.PASRAN_PAYDATE).toDateString();
            $scope.PASRANS_ADMDATE = new Date($scope.PASRANS_ADMDATE).toDateString();
            var data = {
                "PASRAN_ID": $scope.editEmployee,
                "PASRAN_NAME": $scope.PASRAN_NAME,
                "PASRAN_FEEAMOUNT": $scope.PASRAN_FEEAMOUNT,
                "PASRAN_PAYDATE": $scope.PASRAN_PAYDATE,
                "PASRAN_PAYTIME": $filter('date')($scope.PASRAN_PAYTIME, "h:mm a"),             
                "list": $scope.reportdetailsnew,
            };
            apiService.create("PreadmissionNoticeRegistrationReport/Savedata", data).then(function (promise) {
                debugger;
                if (promise.returnresult === true) {
                    if (promise.message == "Saved") {
                        swal('Record Saved Successfully');
                        $state.reload();
                    }
                    else if (promise.message == "Update") {
                        swal('Record Updated Successfully');
                        $state.reload();
                    }
                    else if (promise.message == "Failed") {
                        swal('Record Not Updated Successfully');
                        $state.reload();
                    }
                    else if (promise.message == "Duplicate") {
                        swal('Record Already Exist');
                        $state.reload();
                    }
                    else {
                        swal('Record Not Saved Successfully');
                        $state.reload();
                    }
                }

            });
        }
       
        $scope.viewstudent = function (obj) {

            $scope.uploadstudents = [];
            $scope.uploadstuddetails = [];
            $scope.objdata = obj;
            var data = {
                "PASRAN_ID": obj.pasraN_ID,

            }
            apiService.create("PreadmissionNoticeRegistrationReport/viewstudent", data).then(function (promise) {
                if (promise !== null) {

                    if (promise.uploadstuddetails.length > 0) {
                        $scope.uploadstuddetails = promise.uploadstuddetails;
                    }
                    else {
                        swal('Data Not Available!');
                    }

                }
            });
        };

        $scope.Editdata = function (user) {
           
            var data = {
                "PASRAN_ID": user.pasraN_ID,               
            };
            apiService.create("PreadmissionNoticeRegistrationReport/Editdata", data).
                then(function (promise) {
                  
                    $scope.editEmployee = promise.programlist[0].pasraN_ID;
                    $scope.PASRAN_NAME = promise.programlist[0].pasraN_NAME;
                    $scope.PASRAN_FEEAMOUNT = promise.programlist[0].pasraN_FEEAMOUNT;
                    $scope.PASRAN_PAYDATE = new Date(promise.programlist[0].pasraN_PAYDATE);
                    $scope.PASRAN_PAYTIME = moment(promise.programlist[0].pasraN_PAYTIME, 'h:mm a').format();

                    $scope.reportdetails = promise.reportgrid;
                    if ($scope.reportdetails !== null && $scope.reportdetails.length > 0) {
                        $scope.details = true;
                    } else {
                        $scope.details = false;
                    }
                    $scope.reportids = [];
                    var count = 0;
                    angular.forEach($scope.reportdetails, function (tt) {
                        count = count + 1;
                        tt.sid = count;
                        tt.pasr_id = tt.pasr_id;
                    });

                });
        };
        $scope.printData = function (user) {  
            var data = {
                "PASRAN_ID": user.pasraN_ID
            };            
            apiService.create("PreadmissionNoticeRegistrationReport/printData", data).then(function (promise) {
                $scope.searchstudentDetails2 = [];
                $scope.searchstudentDetails2 = promise.searchstudentDetails2;
                if ($scope.searchstudentDetails2 .length > 0) {                   
                    angular.forEach($scope.searchstudentDetails2, function (aaa) {
                        $scope.name = aaa.pasR_FirstName;
                        $scope.number = aaa.pasR_RegistrationNo;
                        $scope.fee = aaa.pasraN_FEEAMOUNT;
                        $scope.paydate = (aaa.pasraN_PAYDATE, "yyyy-MM-dd");                       
                        $scope.paytime = moment(aaa.pasraN_PAYTIME, 'h:mm a').format();
                        $scope.date = new Date(aaa.pasranS_ADMDATE);
                        $scope.time = moment(aaa.pasranS_TIME, 'h:mm a').format();
                    }); 
                    var innerContents = document.getElementById('printSectionId2').innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/BBKV/BBKVTC/BBKVTCPdf.css" />' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                }               
            });
        };
    }
})();