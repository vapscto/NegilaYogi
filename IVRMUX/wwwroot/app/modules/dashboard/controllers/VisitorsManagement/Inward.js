(function () {
    'use strict';
    angular
        .module('app')
        .controller('InwardController', InwardController)

    InwardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function InwardController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {


        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;


        $scope.search = "";
        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.IW_Id = 0;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.loadgrid = function () {
            apiService.getURI("Inward/getDetails/", 1).then(function (promise) {
                //if (promise.count > 0) {
                $scope.FOIN_DateTime = new Date();

                $scope.emplist1 = promise.emplist;

                $scope.getdataall = promise.getdataall;

                $scope.presentCountgrid = promise.getdataall;
                //}

            });
        }

        //=====================Get emp Record ==================//
        $scope.get_empdetails = function () {
            debugger;
            var empid = $scope.emp1.hrmE_Id;
            // var ascnid = $scope.LMBANO_Id.lmbanO_Id;
            //  $scope.LMB_Id = studid;
            var data = {
                "HRME_Id": empid,

            }
            apiService.create("Inward/get_empdetails", data).then(function (promise) {

                $scope.emplist2 = promise.emplist;

            })
        }

        $scope.get_empdetails2 = function () {
            debugger;
            var empid = $scope.emp1.hrmE_Id;
            var empid2 = $scope.hrmid1.hrmid1;
            //  $scope.LMB_Id = studid;
            var data = {
                "HRME_Id": empid,
                "empid2": empid2,

            }
            apiService.create("Inward/get_empdetails2", data).then(function (promise) {

                $scope.emplist3 = promise.emplist;

            })
        }
        //====================End emp Record ==================//

        //=====================Start emp name Search ==================//

        $scope.searchfilter = function (objj) {
            debugger;
            if (objj.search.length >= '2') {

                var data = {
                    "searchfilter": objj.search,

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("Inward/searchfilter", data).then(function (promise) {

                    $scope.emplist1 = promise.emplist;

                    angular.forEach($scope.emplist1, function (objectt) {
                        if (objectt.hrmE_EmployeeFirstName.length > 0) {
                            var string = objectt.hrmE_EmployeeFirstName;
                            objectt.hrmE_EmployeeFirstName = string.replace(/  +/g, ' ');
                        }
                    })
                })
            }
        }


        $scope.searchfilter2 = function (objj) {
            debugger;
            if (objj.search.length >= '2') {
                var empid = $scope.emp1.hrmE_Id;
                var data = {
                    "searchfilter": objj.search,
                    "HRME_Id": empid,

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("Inward/searchfilter2", data).
                    then(function (promise) {

                        $scope.emplist1 = promise.emplist;

                        angular.forEach($scope.emplist1, function (objectt) {
                            if (objectt.hrmE_EmployeeFirstName.length > 0) {
                                var string = objectt.hrmE_EmployeeFirstName;
                                objectt.hrmE_EmployeeFirstName = string.replace(/  +/g, ' ');
                            }
                        })
                    })
            }

        }


        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.hrmid1 = '';
        $scope.hrmid12 = '';

        $scope.submitted = false;
        $scope.saveData = function () {
            debugger;


            if ($scope.myForm.$valid) {

                //var empid = $scope.emp1.hrmE_Id;
                //var empid2 = $scope.empid2.hrmE_Id;
                //var empid3 = $scope.empid3.hrmE_Id,

                //var date = new Date();
                $scope.time = $filter('date')(new Date(), 'HH:mm:ss');

                var startdate = $scope.FOIN_DateTime == null ? "" : $filter('date')($scope.FOIN_DateTime, "yyyy-MM-dd");

                var obj = {
                    "FOIN_Id": $scope.FOIN_Id,
                    "FOIN_Adddress": $scope.FOIN_Adddress,
                    "FOIN_DateTime": startdate,
                    "FOIN_EmailId": $scope.FOIN_EmailId,
                    "FOIN_PhoneNo": $scope.FOIN_PhoneNo,
                    // "IW_Date": new Date($scope.IW_Date).toDateString(),
                    "FOIN_ContactPerson": $scope.FOIN_ContactPerson,
                    "FOIN_From": $scope.FOIN_From,
                    "FOIN_Discription": $scope.FOIN_Discription,


                    "HRME_Id": $scope.emp1.hrmE_Id,
                    "empid2": $scope.hrmid1.hrmid1,
                    "empid3": $scope.hrmid12.hrmid12,

                }
                apiService.create("Inward/saveData", obj).then(function (promise) {
                    if (promise.returnval != null && promise.duplicate != null) {
                        if (promise.duplicate == false) {
                            if (promise.returnval == "Save") {
                                swal('Record Saved Successfully!!!');

                                $scope.Inwardss = promise.curntInwardrec;
                                $scope.printbutton = false;
                                $scope.SchoolLogo = promise.institution[0].mI_Logo;
                                $scope.SchollName = promise.institution[0].mI_Name;
                                $scope.SchollAdd = promise.institution[0].mI_Address1;
                                $scope.sttud = true;

                                $scope.getdataall.length = 0;
                            }
                            else if (promise.returnval == "Not Save") {
                                swal('Record Not Saved Successfully!!!');
                            }
                            else if (promise.returnval == "Update") {
                                swal('Record Updated Successfully!!!');
                                $state.reload();
                            }
                            else if (promise.returnval == "Not Update") {
                                swal('Record Not Updated Successfully!!!');
                            }
                        }
                        else {
                            swal("Record already exist");
                        }
                    }
                    else {
                        swal("Kindly Contact Administrator!!!");
                    }

                });
            }
            else {
                $scope.submitted = true;
            }

        }

        $scope.cancel = function () {
            $scope.FOIN_Id = 0;
            $scope.FOIN_Discription = "";
            $scope.FOIN_DateTime = "";
            $scope.FOIN_From = "";
            $scope.FOIN_ContactPerson = "";
            $scope.FOIN_PhoneNo = "";
            $scope.FOIN_EmailId = "";
            $scope.FOIN_Adddress = "";
            $scope.sttud = false;
            $scope.printbutton = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.emp1 = "";
            $scope.hrmid1 = "";
            $scope.hrmid12 = "";

            $state.reload();

        };

        $scope.printbutton = true;
        $scope.Print = function () {
            var innerContents = '';
            innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/OutwardPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        }

        $scope.EditDetails = function (data) {
            debugger;
            var data = {
                "FOIN_Id": data.foiN_Id,
            }
            apiService.create("Inward/EditDetails/", data).then(function (promise) {

                $scope.editDetails = promise.editDetails;
                $scope.FOIN_Id = promise.editDetails[0].foiN_Id;
                $scope.FOIN_Discription = promise.editDetails[0].foiN_Discription;
                $scope.FOIN_From = promise.editDetails[0].foiN_From;
                $scope.FOIN_EmailId = promise.editDetails[0].foiN_EmailId;
                $scope.FOIN_PhoneNo = promise.editDetails[0].foiN_PhoneNo;
                $scope.FOIN_DateTime = new Date(promise.editDetails[0].foiN_DateTime);
                $scope.FOIN_ContactPerson = promise.editDetails[0].foiN_ContactPerson;
                $scope.FOIN_Adddress = promise.editDetails[0].foiN_Adddress;


                $scope.emp1 = promise.editDetails[0];

                $scope.emp1.hrmE_Id = promise.editDetails[0].foiN_To;

                $scope.get_empdetails($scope.emp1.hrmE_Id);

                $scope.hrmid1 = promise.emplist2[0];
                if ($scope.hrmid1 != null || $scope.hrmid1 != undefined) {
                    if ($scope.hrmid1.length > 0) {
                        $scope.hrmid1.hrmid1 = promise.emplist2[0].hrmid1;
                    }
                }
                else {
                    $scope.hrmid1 = '';
                }
                $scope.employeename1 = promise.emplist2[0].employeename1;

                $scope.hrmid12 = promise.emplist3[0];

                if ($scope.hrmid12 != null || $scope.hrmid12 != undefined) {
                    if ($scope.hrmid12.length > 0) {
                        $scope.hrmid12.hrmid12 = promise.emplist3[0].hrmid12;
                    }
                }
                else {
                    $scope.hrmid12 = '';
                }
                $scope.employeename12 = promise.emplist3[0].employeename12;



            });
        }

        $scope.deactive = function (newuser1, SweetAlertt) {
            var mgs = "";
            if (newuser1.foiN_ActiveFlag == false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Record?",
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
                        apiService.create("Inward/deactivate", newuser1).
                            then(function (promise) {
                                if (promise.returnval2 == true) {

                                    swal("Record " + mgs + "d Successfully!!!");

                                }
                                else {
                                    swal("Record Not " + mgs + "d Successfully!!!");

                                }
                                $state.reload();
                            })

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }

                });
        }


        $scope.interacted = function (field) {
            return $scope.submitted;
        };





        ////////////////===================================Old Code
        //$scope.searchValue = "";
        //$scope.currentPage = 1;
        //$scope.itemsPerPage = 10;
        //$scope.IW_Id = 0;
        //$scope.sort = function (key) {
        //    $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
        //    $scope.sortKey = key;
        //}

        //$scope.loadgrid = function () {
        //    apiService.getURI("Inward/getDetails/", 1).then(function (promise) {
        //        if (promise.count > 0) {

        //            $scope.gridoptions = promise.gridoptions;
        //            $scope.presentCountgrid = promise.gridoptions.length;
        //        }

        //        $scope.cancel();
        //    });
        //}

        //$scope.hstep = 1;
        //$scope.mstep = 1;

        //$scope.submitted = false;
        //$scope.saveData = function () {
        //    if ($scope.myForm.$valid) {

        //        if ($scope.Ass_To == undefined || $scope.Ass_To == null) {
        //            $scope.Ass_To = 0;
        //        }

        //        var date = new Date();
        //        $scope.time = $filter('date')(new Date(), 'HH:mm:ss'); 

        //        var obj = {
        //            "IW_Id" : $scope.IW_Id,
        //            "IW_Discription": $scope.IW_Discription,
        //            "IW_From": $scope.IW_From,
        //            "IW_To": $scope.IW_To,
        //            "Ass_To": $scope.Ass_To,
        //            "IW_No": $scope.IW_No,
        //            "IW_Date": new Date($scope.IW_Date).toDateString(),
        //            "IW_Remarks": $scope.IW_Remarks
        //        }
        //        apiService.create("Inward/saveData", obj).
        //            then(function (promise) {
        //                if (promise.returnVal == 'saved') {
        //                    swal("Record Saved Successfully");
        //                    $scope.inward = promise.inward;
        //                    $scope.sttud = true;
        //                    $scope.screport = true;
        //                    $scope.SchoolLogo = promise.institution[0].mI_Logo;
        //                    $scope.SchollName = promise.institution[0].mI_Name;
        //                    $scope.SchollAdd = promise.institution[0].mI_Address1;

        //                }
        //                else if (promise.returnVal == 'updated') {
        //                    swal("Record Updated Successfully");
        //                    $scope.loadgrid();
        //                }
        //                else if (promise.returnVal == 'duplicate') {
        //                    swal("Record already exist");
        //                }
        //                else if (promise.returnVal == "savingFailed") {
        //                    swal("Failed to save record");
        //                }
        //                else if (promise.returnVal == "updateFailed") {
        //                    swal("Failed to update record");
        //                }
        //                else {
        //                    swal("Sorry...something went wrong");
        //                }

        //            });
        //    }
        //    else {
        //        $scope.submitted = true;
        //    }
        //}


        //$scope.cancel = function () {
        //    $scope.IW_Id = 0;
        //    $scope.IW_Discription = "";
        //    $scope.IW_From = "";
        //    $scope.IW_To = "";
        //    $scope.Ass_To = "";
        //    $scope.IW_No = "";
        //    $scope.IW_Date = "";
        //    $scope.IW_Remarks = "";  
        //    $scope.sttud = false;
        //    $scope.screport = false;
        //    $scope.submitted = false;
        //    $scope.myForm.$setPristine();
        //    $scope.myForm.$setUntouched();

        //};

        //$scope.Print = function () {
        //    var innerContents = '';
        //    innerContents = document.getElementById("printSectionId").innerHTML;
        //    var popupWinindow = window.open('');
        //    popupWinindow.document.open();
        //    popupWinindow.document.write('<html><head>' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
        //        '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
        //        '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
        //    );
        //    popupWinindow.document.close();
        //}

        //$scope.edit = function (data) {
        //    $scope.IW_Id = data;
        //    apiService.getURI("Inward/edit/", $scope.IW_Id).then(function (promise) {
        //        $scope.editDetails = promise.editDetails;
        //        $scope.IW_Discription = promise.editDetails[0].iW_Discription;
        //        $scope.IW_From = promise.editDetails[0].iW_From;
        //        $scope.IW_To = promise.editDetails[0].iW_To;
        //        $scope.IW_No = promise.editDetails[0].iW_No;
        //        $scope.IW_Date = new Date(promise.editDetails[0].iW_Date);
        //        $scope.IW_Remarks = promise.editDetails[0].iW_Remarks;
        //        $scope.Ass_To = promise.editDetails[0].ass_To;
        //    });
        //}

        //$scope.deactive = function (newuser1, SweetAlertt) {
        //    var mgs = "";
        //    if (newuser1.iW_ActiveFlag == false) {
        //        mgs = "Activate";
        //    } else {
        //        mgs = "Deactivate";
        //    }
        //    swal({
        //        title: "Are you sure?",
        //        text: "Do you want to  " + mgs + " the Record?",
        //        type: "warning",
        //        showCancelButton: true,
        //        confirmButtonColor: "#DD6B55",
        //        confirmButtonText: "Yes, " + mgs + " it!",
        //        cancelButtonText: "Cancel..!",
        //        closeOnConfirm: false,
        //        closeOnCancel: false
        //    },
        //        function (isConfirm) {
        //            if (isConfirm) {
        //                apiService.create("Inward/deactivate", newuser1).
        //                    then(function (promise) {
        //                        if (promise.value == true) {
        //                            if (promise.msg != null) {
        //                                swal(promise.msg);
        //                                $scope.loadgrid();
        //                            }
        //                        }
        //                        else {
        //                            swal('Failed to Activate/Deactivate the Record');
        //                        }
        //                    })
        //            } else {
        //                swal("Cancelled");
        //            }
        //        })
        //}

        //$scope.interacted = function (field) {
        //    return $scope.submitted;
        //};

    }

})();