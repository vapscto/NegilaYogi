

(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeTrailAuditReportController', FeeTrailAuditReport123)

    FeeTrailAuditReport123.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter']
    function FeeTrailAuditReport123($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {

        $scope.temp_del_rpts = [];
        $scope.cfg = {};
        $scope.Userstatus_flag = false;
        var institutionid = "";
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        institutionid = configsettings[0].mI_Id;

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        $scope.order = function (keyname) {
            $scope.predicate = keyname;   //set the sortKey to the param passed
            $scope.default = !$scope.default; //if true make it false and vice versa
        };
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;

            var pageid = 2;
            apiService.getURI("FeeTrailAuditReport/getalldetails123", pageid).
                then(function (promise) {

                    $scope.yearlst = promise.yearlist;

                    $scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;

                    //$scope.SU_Receipts = promise.newreplist;
                    //$scope.SU_Students = promise.admsudentslist;
                    //angular.forEach(promise.d_Receipts, function (rt) {
                    //    $scope.temp_del_rpts.push({ paymentid: rt, receiptNo: rt });
                    //})
                    //promise.d_Receipts=$scope.temp_del_rpts;
                    //$scope.Del_Receipts = promise.d_Receipts;
                    //$scope.Del_Students = promise.d_Students;
                    //$scope.clear_status();
                    //$scope.onclickloaddata();
                });
        };

        $scope.onclickloaddata = function () {
            if ($scope.usercheck == "1") {
                $scope.checked = false;
            }
            else {
                $scope.checked = true;
            }
            if ($scope.studentcheck == "1") {
                $scope.studentcheckdrp = false;
            }
            else {
                $scope.studentcheckdrp = true;
            }
            if ($scope.recp == "1") {
                $scope.recpdrp = false;

            }
            else {
                $scope.recpdrp = true;
            }
            if ($scope.betdates == "1") {

                $scope.frmdated = false;
                $scope.todated = false;
            }
            else {
                $scope.frmdated = true;
                $scope.todated = true;
            }
        };

        $scope.ShowReportdata = function () {

            if ($scope.myForm.$valid) {
                var data = "";

                if ($scope.rpttyp === 'receipt') {
                    data = {
                        "Report_Type": $scope.rpttyp,
                        "receiptNo": $scope.paymentid,
                        "Save_flag": $scope.Save_flag,
                        "Update_flag": $scope.Update_flag,
                        //"Delete_flag": $scope.Delete_flag,
                        "Status_IU_D": $scope.statustyp,
                        "fromdate": new Date().toDateString(),
                        "todate": new Date().toDateString(),
                        "User_Status": $scope.Userstatus_flag
                    };
                }
                else if ($scope.rpttyp === 'studentttt') {
                    data = {
                        "Report_Type": $scope.rpttyp,
                        "Amst_Id": $scope.Amst_Id.amst_Id,
                        "Save_flag": $scope.Save_flag,
                        "Update_flag": $scope.Update_flag,
                        //"Delete_flag": $scope.Delete_flag,
                        "Status_IU_D": $scope.statustyp,
                        "fromdate": new Date().toDateString(),
                        "todate": new Date().toDateString(),
                        "User_Status": $scope.Userstatus_flag
                    };
                }
                else if ($scope.rpttyp === 'date') {
                    data = {
                        "Report_Type": $scope.rpttyp,
                        "fromdate": new Date($scope.fromdate).toDateString(),
                        "todate": new Date($scope.todate).toDateString(),
                        "Save_flag": $scope.Save_flag,
                        "Update_flag": $scope.Update_flag,
                        //"Delete_flag": $scope.Delete_flag,
                        "Status_IU_D": $scope.statustyp,
                        "User_Status": $scope.Userstatus_flag
                    };
                }

                apiService.create("FeeTrailAuditReport/getreport", data).
                    then(function (promise) {
                        if (promise.reportdatelist.length > 0) {

                            var omar = new Date();
                            var y = omar.getFullYear();
                            var m = omar.getMonth();
                            var d = omar.getDate();

                            var date = y + '-' + m + '-' + d;

                            $scope.Main_Details = promise.reportdatelist;
                            angular.forEach($scope.Main_Details, function (tt) {

                                tt.ITAT_Time = new Date(date + ' ' + tt.ITAT_Time);
                            });
                        }
                        else {
                            promise.reportdatelist.length = 0;
                            swal("Record Not Found");
                        }

                        //if (promise.main_Details != null && promise.main_Details != "")
                        //{
                        //    $scope.Main_Details = promise.main_Details;
                        //    angular.forEach($scope.Main_Details, function (yt) {
                        //        yt.itaT_Time = yt.itaT_Time.substring(0, 8);

                        //        if(yt.fyP_Bank_Or_Cash=='B')
                        //        {
                        //            yt.fyP_Bank_Or_Cash = 'Bank';
                        //        }
                        //        else if (yt.fyP_Bank_Or_Cash == 'C') {
                        //            yt.fyP_Bank_Or_Cash = 'Cash';
                        //        }
                        //        else if (yt.fyP_Bank_Or_Cash == 'R') {
                        //            yt.fyP_Bank_Or_Cash = 'RTGS/NTF';
                        //        }
                        //        else if (yt.fyP_Bank_Or_Cash == 'S') {
                        //            yt.fyP_Bank_Or_Cash = 'Card';
                        //        }
                        //        if (yt.itaT_Operation == 'I')
                        //        {
                        //            yt.itaT_Operation = 'Insert';
                        //        }
                        //        else if (yt.itaT_Operation == 'U') {
                        //            yt.itaT_Operation = 'Update';
                        //        }
                        //        else if (yt.itaT_Operation == 'D') {
                        //            yt.itaT_Operation = 'Delete';
                        //        }
                        //    })
                        //    $scope.Report_List = promise.reportdatelist;
                        //}
                        //else {
                        //    swal("Record Not Found");
                        //    $scope.Main_Details = [];
                        //    $scope.Report_List = [];
                        //}


                    });
            }
            else {
                $scope.submitted = true;

            }
        };


        // View Details
        $scope.viewdetails = function (user) {

            var data = {
                "Report_Type": $scope.rpttyp,
                "fromdate": new Date($scope.fromdate).toDateString(),
                "todate": new Date($scope.todate).toDateString(),
                "Save_flag": $scope.Save_flag,
                "Update_flag": $scope.Update_flag,
                "Status_IU_D": $scope.statustyp,
                "User_Status": $scope.Userstatus_flag,
                "FYP_Id": user.FYP_Id
            };

            apiService.create("FeeTrailAuditReport/viewdetails", data).then(function (promise) {

                if (promise !== null) {
                    $scope.view_details = promise.getviewdetails;

                    angular.forEach($scope.view_details, function (dd) {

                        if (dd.IATD_ColumnName === "FYP_Bank_Name") {
                            dd.columname = "Bank Name";
                        } else if (dd.IATD_ColumnName === "FYP_Bank_Or_Cash") {
                            dd.columname = "Mode Of Payment";
                        } else if (dd.IATD_ColumnName === "FYP_DD_Cheque_No") {
                            dd.columname = "Cheque No";
                        } else if (dd.IATD_ColumnName === "FYP_DD_Cheque_Date") {
                            dd.columname = "Cheque Date";
                        } else if (dd.IATD_ColumnName === "FYP_Date") {
                            dd.columname = "Transaction Date";
                        } else if (dd.IATD_ColumnName === "FYP_Tot_Amount") {
                            dd.columname = "Transaction Amount";
                        } else if (dd.IATD_ColumnName === "FYP_Remarks") {
                            dd.columname = "Remarks";
                        } else if (dd.IATD_ColumnName === "FYP_Receipt_No") {
                            dd.columname = "Receipt No";
                        }
                           else if (dd.IATD_ColumnName === "fyp_transaction_id") {
                            dd.columname = "Transaction Id (If mode of Payment is Online)";
                        }
                        else if (dd.IATD_ColumnName === "FYP_PaymentReference_Id") {
                            dd.columname = "Payment Reference Id (If mode of Payment is Online)";
                        }
                    });

                    console.log($scope.view_details);
                } else {
                    swal("Something Went Wrong Contact Administrator");
                }

            });
        };


        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.Save_flag = true;
        $scope.Update_flag = true;
        // $scope.Delete_flag = true;
        $scope.clear = function () {
            $scope.rpttyp = 'receipt';
            $scope.statustyp = 'IU';
            $scope.Save_flag = true;
            $scope.Update_flag = true;
            // $scope.Delete_flag = true;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.clear_status();
            //  $scope.clear_valids();
        };

        $scope.clear_valids = function () {
            $scope.fromdate = null;
            $scope.todate = null;
            $scope.Amst_Id = "";
            $scope.paymentid = "";

            $scope.Save_flag = true;
            $scope.Update_flag = true;
            // $scope.Delete_flag = true;
            $scope.search = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.Main_Details = [];
            $scope.Report_List = [];
            $scope.printdatatable = [];
            $scope.stdall = false;

        };

        //$scope.viewdetails = function (user) {
        //    $scope.view_details = [];
        //    $scope.view_type = user.itaT_Operation;
        //    $scope.Receipt_No = user.fyP_Receipt_No;
        //    $scope.Name = user.name;
        //    angular.forEach($scope.Main_Details, function (rt) {
        //        if (rt.ITAT_Id === user.ITAT_Id) {
        //            $scope.view_details.push(rt);
        //        }
        //    });
        //    console.log($scope.view_details);
        //};





        $scope.chk_flags_s = function (flag) {
            if (flag === false) {
                if ($scope.Save_flag === false && $scope.Update_flag === false) {
                    $scope.Update_flag = true;
                    // $scope.Save_flag = true;
                }
            }
        };
        $scope.chk_flags_u = function (flag) {
            if (flag === false) {
                if ($scope.Save_flag === false && $scope.Update_flag === false) {
                    // $scope.Update_flag = true;
                    $scope.Save_flag = true;
                }
            }
        };

        $scope.clear_status = function () {
            if ($scope.statustyp === 'IU') {
                $scope.studentlst = $scope.SU_Students;
                $scope.receiptlistarray = $scope.SU_Receipts;
            }
            else if ($scope.statustyp === 'D') {
                $scope.studentlst = $scope.Del_Students;
                $scope.receiptlistarray = $scope.Del_Receipts;
            }
            $scope.clear_valids();
        };
        $scope.printdatatable = [];
        $scope.toggleAllstd = function () {

            var toggleStatus = $scope.stdall;
            $scope.printdatatable = [];
            angular.forEach($scope.Main_Details, function (itm) {
                itm.stdselected = toggleStatus;
                if ($scope.stdall === true) {
                    $scope.printdatatable.push(itm);
                }
            });
            if ($scope.printdatatable.length !== null && $scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }

        };
        $scope.optionToggledstd = function (SelectedStudentRecord, index) {

            $scope.stdall = $scope.Main_Details.every(function (itm) { return itm.stdselected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }

        };
        $scope.exportToExcel = function (Table_p) {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var excelname = "Fee Trail Audit Report";
                var exportHref = Excel.tableToExcel(Table_p, 'Fee Trail Audit Report');
                //$timeout(function () { location.href = exportHref; }, 100);
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelname;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }
        };

        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');


        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.printData = function () {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                    '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                    '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }

        };

        $scope.studentlst = [];

        $scope.searchfilter = function (objj, radioobj) {
            var data = "";
            if (institutionid == '5' || institutionid == '4' || institutionid == '3' || institutionid == '6' || institutionid == '8' || institutionid == '10001') {
                if (objj.search.length >= '2' && radioobj === 'studentttt') {

                    data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "asmay_id": $scope.cfg.ASMAY_Id
                    };

                    apiService.create("FeeTrailAuditReport/searchfilter", data).
                        then(function (promise) {

                            $scope.studentlst = promise.fillstud;

                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            });
                            if ($scope.filterdata === 'Challan_No') {

                                angular.forEach($scope.studentlst, function (ob) {
                                    if (ob.amst_Id == temp_amst_Id) {
                                        ob.Selected = true;
                                        $scope.Amst_Id = ob;
                                    }
                                });

                                //$scope.onselectstudent($scope.Amst_Id);
                            }
                        });
                }


                if (objj.search.length >= '3' && radioobj === 'receipt') {

                    data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "asmay_id": $scope.cfg.ASMAY_Id
                    };

                    apiService.create("FeeTrailAuditReport/searchfilter", data).
                        then(function (promise) {

                            $scope.studentrecelst = promise.fillstud;

                            //angular.forEach($scope.studentrecelst, function (objectt) {
                            //    if (objectt.fyP_Receipt_No.length > 0) {
                            //        var string = objectt.fyP_Receipt_No;
                            //        objectt.fyP_Receipt_No = string.replace(/  +/g, ' ');
                            //    }
                            //})

                        });
                }

            }
        };

    }
})();

