(function () {
    'use strict';
    angular
        .module('app')
        .controller('OutwardController', OutwardController)

    OutwardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function OutwardController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {

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



        $scope.sttud = false;
        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.loadgrid = function () {
            apiService.getURI("Outward/getDetails/", 1).then(function (promise) {
                $scope.FOOUT_DateTime = new Date(); 
                // if (promise.count > 0) {
                $scope.emplist = promise.emplist;
                $scope.getdata = promise.getdata;
                //$scope.gridoptions = promise.gridoptions;
                //$scope.presentCountgrid = promise.gridoptions.length;
                //  }

                // $scope.cancel();
            });
        }

        $scope.submitted = false;
        $scope.saveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var date = new Date();
                $scope.time = $filter('date')(new Date(), 'HH:mm:ss');
                
                var obj = {
                    "FOOUT_Id": $scope.FOOUT_Id,
                    //"FOOUT_OutwardNo": $scope.FOOUT_OutwardNo,
                    //"FOOUT_DateTime": $scope.FOOUT_DateTime,
                    "FOOUT_DateTime": new Date($scope.FOOUT_DateTime).toDateString(),
                    "FOOUT_Discription": $scope.FOOUT_Discription,
                    "FOOUT_From": $scope.FOOUT_From,
                    "FOOUT_To": $scope.FOOUT_To,
                    "FOOUT_Address": $scope.FOOUT_Address,
                    "FOOUT_PhoneNo": $scope.FOOUT_PhoneNo,
                    "FOOUT_EmailId": $scope.FOOUT_EmailId,
                    "FOOUT_DispatachedBy": $scope.hrmE_Id.hrmE_Id,
                    "FOOUT_DispatchedThrough": $scope.FOOUT_DispatchedThrough,
                    "FOOUT_DispatchedDeatils": $scope.FOOUT_DispatchedDeatils,
                    "FOOUT_DispatchedPhNo": $scope.FOOUT_DispatchedPhNo,

                }
                apiService.create("Outward/saveData", obj).
                    then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == "Save") {
                                    swal('Record Saved Successfully!!!');

                                    $scope.outward = promise.outward;
                                    $scope.printbutton = false;
                                    $scope.SchoolLogo = promise.institution[0].mI_Logo;
                                    $scope.SchollName = promise.institution[0].mI_Name;
                                    $scope.SchollAdd = promise.institution[0].mI_Address1;
                                    $scope.sttud = true;  

                                    $scope.getdata.length = 0;
                                }
                                else if (promise.returnval == "Not Save")  {
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
            $scope.OW_Id = 0;
            $scope.OW_Discription = "";
            $scope.OW_From = "";
            $scope.OW_To = "";
            $scope.OW_add = "";
            $scope.OW_Date = "";
            $scope.OW_Remarks = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.printbutton = true;
            $scope.sttud = false;
            $state.reload();

        };

        $scope.edit = function (data) {
            
            var data = {
                "FOOUT_Id": data.foouT_Id
            }
            apiService.create("Outward/EditDetails/", data).then(function (promise) {
                
                $scope.editDetails = promise.editDetails;
                $scope.FOOUT_Id = promise.editDetails[0].foouT_Id;
                $scope.FOOUT_OutwardNo = promise.editDetails[0].foouT_OutwardNo;
                $scope.FOOUT_PhoneNo = promise.editDetails[0].foouT_PhoneNo;
                $scope.FOOUT_From = promise.editDetails[0].foouT_From;
                $scope.FOOUT_To = promise.editDetails[0].foouT_To;
                $scope.FOOUT_DateTime = new Date(promise.editDetails[0].foouT_DateTime);
               
                $scope.FOOUT_Discription = promise.editDetails[0].foouT_Discription;
                
                $scope.FOOUT_DispatchedDeatils = promise.editDetails[0].foouT_DispatchedDeatils;
                $scope.FOOUT_DispatchedPhNo = promise.editDetails[0].foouT_DispatchedPhNo;
                $scope.FOOUT_DispatchedThrough = promise.editDetails[0].foouT_DispatchedThrough;
                $scope.FOOUT_EmailId = promise.editDetails[0].foouT_EmailId;
                $scope.FOOUT_Address = promise.editDetails[0].foouT_Address;

                $scope.hrmE_Id = promise.editDetails[0];

                $scope.hrmE_Id.hrmE_Id = promise.editDetails[0].foouT_DispatachedBy;

            });
        }


        $scope.deactive = function (newuser1, SweetAlertt) {
            var mgs = "";
            if (newuser1.foouT_ActiveFlag == false) {
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
                        apiService.create("Outward/deactivate", newuser1).
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



        /////////////////////////////////////////////////////======================Old Code

        //$scope.sttud = false; 
        //$scope.searchValue = "";
        //$scope.currentPage = 1;
        //$scope.itemsPerPage = 10;
        //$scope.OW_Id = 0;
        //$scope.sort = function (key) {
        //    $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
        //    $scope.sortKey = key;
        //}

        //$scope.loadgrid = function () {
        //    apiService.getURI("Outward/getDetails/", 1).then(function (promise) {
        //        if (promise.count > 0) {

        //            $scope.gridoptions = promise.gridoptions;
        //            $scope.presentCountgrid = promise.gridoptions.length;
        //        }

        //       // $scope.cancel();
        //    });
        //}

        //$scope.submitted = false;
        //$scope.saveData = function () {
        //    debugger
        //    if ($scope.myForm.$valid) {

        //        var date = new Date();
        //        $scope.time = $filter('date')(new Date(), 'HH:mm:ss'); 

        //        var obj = {
        //            "OW_Id": $scope.OW_Id,
        //            "OW_Discription": $scope.OW_Discription,
        //            "OW_From": $scope.OW_From,
        //            "OW_To": $scope.OW_To,
        //            "OW_add": $scope.OW_add,
        //            "OW_Date": new Date($scope.OW_Date),
        //            "OW_Remarks": $scope.OW_Remarks
        //        }
        //        apiService.create("Outward/saveData", obj).
        //            then(function (promise) {
        //                if (promise.returnVal == 'saved') {
        //                    swal("Record Saved Successfully");
        //                    $scope.outward = promise.outward;
        //                    $scope.screport = true;

        //                    $scope.SchoolLogo = promise.institution[0].mI_Logo;
        //                    $scope.SchollName = promise.institution[0].mI_Name;
        //                    $scope.SchollAdd = promise.institution[0].mI_Address1;
        //                    $scope.loadgrid();   
        //                    $scope.sttud = true; 
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
        //                $scope.loadgrid();
        //            });
        //    }
        //    else {
        //        $scope.submitted = true;
        //    }
        //} 

        //$scope.cancel = function () {
        //    $scope.OW_Id = 0;
        //    $scope.OW_Discription = "";
        //    $scope.OW_From = "";
        //    $scope.OW_To = "";
        //    $scope.OW_add = "";
        //    $scope.OW_Date = "";
        //    $scope.OW_Remarks = "";
        //    $scope.submitted = false;
        //    $scope.myForm.$setPristine();
        //    $scope.myForm.$setUntouched();
        //    $scope.screport = false;
        //    $scope.sttud = false;
        //    $state.reload();

        //};

        //$scope.edit = function (data) {
        //    debugger;
        //    $scope.OW_Id = data;
        //    apiService.getURI("Outward/edit/", $scope.OW_Id).then(function (promise) {
        //        debugger
        //        $scope.editDetails = promise.editDetails;
        //        $scope.OW_Discription = promise.editDetails[0].oW_Discription;
        //        $scope.OW_From = promise.editDetails[0].oW_From;
        //        $scope.OW_To = promise.editDetails[0].oW_To;
        //        $scope.OW_add = promise.editDetails[0].oW_add;
        //        $scope.OW_Date = new Date(promise.editDetails[0].oW_Date);
        //        $scope.OW_Remarks = promise.editDetails[0].oW_Remarks;
        //    });
        //}


        //$scope.deactive = function (newuser1, SweetAlertt) {
        //    var mgs = "";
        //    if (newuser1.oW_ActiveFlag == false) {
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
        //                apiService.create("Outward/deactivate", newuser1).
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


    }

})();