(function () {
    'use strict';
    angular
        .module('app')
        .controller('ThirdPartyTransactionController', ThirdPartyTransactionController)

    ThirdPartyTransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter']
    function ThirdPartyTransactionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter) {

        $scope.Show_Others = false;
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.cfg = {};
        $scope.fyP_DD_Cheque_Date = new Date();
        $scope.FYP_Date = new Date();

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
            var instituteid = admfigsettings[0].mI_Id;
        }

        $scope.imgname = logopath;
        $scope.istitute_id = instituteid;

        $scope.currentPage = 1;
        $scope.itemsPerPage = 4;

        //------Search for date  From search Table
        $scope.search = '';
        $scope.filterValue = function (obj) {

            return
            angular.lowercase(obj.fyptP_Name).indexOf(angular.lowercase($scope.search)) >= 0 ||
                angular.lowercase(obj.fmH_FeeName).indexOf(angular.lowercase($scope.search)) >= 0 ||

                angular.lowercase(obj.asmaY_Year).indexOf(angular.lowercase($scope.search)) >= 0 ||

                angular.lowercase(obj.fyP_Receipt_No).indexOf(angular.lowercase($scope.search)) >= 0 ||

                //angular.lowercase(obj.fyP_Tot_Amount).indexOf(angular.lowercase($scope.search)) >= 0 ||

                angular.lowercase(obj.fyptP_Towards).indexOf(angular.lowercase($scope.search)) >= 0 ||

                angular.lowercase(obj.fyP_Bank_Or_Cash).indexOf(angular.lowercase($scope.search)) >= 0 ||
                ($filter('date')(obj.fyP_Date, 'dd/MM/yyyy').indexOf($scope.search) >= 0)
        }
        //===================End----//

        //-----------start loaddata---------
        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;


            var pageid = 2;
            apiService.getURI("ThirdPartyTransaction/getdetails", pageid).
                then(function (promise) {

                    //  $scope.studntlist = promise.studntlist;
                    $scope.yearlist = promise.yearlist;
                    $scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;
                    $scope.Alldata = promise.alldata;
                    $scope.fyP_Receipt_No = promise.fyP_Receipt_No;

                    $scope.feegrouplist = promise.feegrouplist;
                    $scope.grouplist = promise.grouplist;

                    $scope.feebankdetails = promise.stdetails;

                    if (promise.thirdparty_auto_receipt.length > 0) {
                        $scope.showreceiptno = false;
                    }
                    else {
                        $scope.showreceiptno = true;
                    }
                })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
        //============End Load Data----------//


        //---------------for Edit record.........
        $scope.editOthtransaction = function (fyP_Id, fyptP_ID) {
            $scope.receipt = true;
            debugger;
            var data = {
                "FYP_ID": fyP_Id,
                "FYPTP_ID": fyptP_ID
            }
            apiService.create("ThirdPartyTransaction/editOthtransaction", data).
                then(function (promise) {
                    debugger;
                    if (promise.editOther.length > 0) {
                        $scope.fyP_Id = fyP_Id;
                        $scope.cfg.ASMAY_Id = promise.editOther[0].asmaY_Id;
                        $scope.cfg.FYPTP_Name = promise.editOther[0].fyptP_Name;
                        // $scope.fmH_Id = promise.editOther[0].fmH_Id;
                        $scope.fyP_Receipt_No = promise.editOther[0].fyP_Receipt_No;
                        $scope.fyP_Tot_Amount = promise.editOther[0].fyP_Tot_Amount;
                        $scope.FYP_Remarks = promise.editOther[0].fyP_Remarks;
                        $scope.FYP_Date = promise.editOther[0].fyP_Date;
                        $scope.cfg.fyP_Bank_Name = promise.editOther[0].fyP_Bank_Name;
                        $scope.fyP_DD_Cheque_Date = promise.editOther[0].fyP_DD_Cheque_Date;
                        $scope.fyP_DD_Cheque_No = promise.editOther[0].fyP_DD_Cheque_No;
                        $scope.getgroup();
                        $scope.fmH_Id = promise.editOther[0].fmH_Id;

                        $scope.filterdata2 = promise.editOther[0].fyP_Bank_Or_Cash;

                    }

                })
        }
        //====================End Edit record.............!!!!

        //---------------for Delete record......
        $scope.DeletOthrRecord = function (fyP_Id, fyptP_ID) {
            var data = {
                "FYP_ID": fyP_Id,
                "FYPTP_ID": fyptP_ID
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false,
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ThirdPartyTransaction/DeletOthrRecord", data).then(function (promise) {
                            if (promise.returnval = true) {
                                swal("Record Deleted Successfully!!!");
                                $state.reload();
                            }
                            else if (promise.returnval = false) {
                                swal("Record Not Deleted Successfully!!!");
                            }

                        })
                    }
                    else {
                        swal("Record Deletation Cancel!!!");
                    }

                })
        }
        //===========End-Delete record..................!!!!



        //=====get group
        $scope.getgroup = function () {
            debugger;
            //if ($scope.fyP_Id > 0)
            //{

            //}
            //else {
            //    $scope.fyP_Receipt_No = "";
            //}

            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id
            }
            apiService.create("ThirdPartyTransaction/getgrpdetails", data).
                then(function (promise) {
                    debugger;
                    $scope.studntlist = promise.studntlist;
                    //$scope.grouplist = promise.grouplist;
                    //$scope.feegrouplist = promise.feegrouplist;
                })
            var asmaY_Year = "";
            angular.forEach($scope.yearlist, function (yr) {
                if (yr.asmaY_Id == $scope.cfg.ASMAY_Id) {
                    asmaY_Year = yr.asmaY_Year;
                }
            })
            $scope.getdates($scope.cfg.ASMAY_Id, asmaY_Year);
        }
        //==============End=============//


        //====Get Selected Student Data======//
        $scope.onselectstudent = function () {
            debugger;
            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                "AMST_Id": $scope.amsT_Id

            }
            apiService.create("ThirdPartyTransaction/getStudtdetails", data).
                then(function (promise) {
                    debugger;
                    $scope.studinfo = promise.studinfo;

                })
        }
        //==============End=============//


        //========Clear========//
        $scope.cleardata = function () {
            $state.reload();
        }
        //==============End=============//


        //===Check for Duplicate data===//
        $scope.Ckeck_Receipt = function () {
            debugger;
            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                "FYP_Receipt_No": $scope.fyP_Receipt_No
            }
            apiService.create("ThirdPartyTransaction/Ckeck_Receipt", data).
                then(function (promise) {
                    debugger;
                    if (promise.returnval) {
                        swal("Receipt No Should Not be Duplicate!!!");
                        $scope.fyP_Receipt_No = "";
                    }
                })
        }
        //==============End=============//


        //=========For Validation=============//
        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted
        };
        //==============End=============//



        //========Save Data for Selected Group=============//
        $scope.SaveStudentgroupdata = function () {

            debugger;
            if ($scope.myForm.$valid) {

                if ($scope.filterdata3 == "other") {
                    $scope.FYPTP_Name = $scope.cfg.FYPTP_Name;
                }
                else if ($scope.filterdata3 == "student") {
                    $scope.FYPTP_Name = $scope.cfg.FYPTP_Name.AMCST_FirstName;
                }


                if ($scope.fyP_Id > 0) {


                    var data = {
                        "ASMAY_ID": $scope.cfg.ASMAY_Id,
                        "FYP_Id": $scope.fyP_Id,
                        "FYPTP_Name": $scope.FYPTP_Name,
                        "FMH_Id": $scope.fmH_Id,
                        "FYP_Receipt_No": $scope.fyP_Receipt_No,
                        "FYP_Tot_Amount": $scope.fyP_Tot_Amount,
                        "FYP_Bank_Or_Cash": $scope.filterdata2,
                        "FYP_Bank_Name": $scope.cfg.fyP_Bank_Name,
                        "FYP_DD_Cheque_Date": new Date($scope.fyP_DD_Cheque_Date).toDateString(),
                        "FYP_DD_Cheque_No": $scope.fyP_DD_Cheque_No,
                        "FYP_Date": new Date($scope.FYP_Date).toDateString(),
                        "FYP_Remarks": $scope.FYP_Remarks,
                        "FMG_Id": $scope.fmG_Id,
                    }
                }
                else {
                    var data = {
                        "ASMAY_ID": $scope.cfg.ASMAY_Id,
                        //"AMST_Id": $scope.amsT_Id,
                        "FYPTP_Name": $scope.FYPTP_Name,
                        "FMH_Id": $scope.fmH_Id,
                        "FYP_Receipt_No": $scope.fyP_Receipt_No,
                        "FYP_Tot_Amount": $scope.fyP_Tot_Amount,
                        "FYP_Bank_Or_Cash": $scope.filterdata2,
                        "FYP_Bank_Name": $scope.cfg.fyP_Bank_Name,
                        "FYP_DD_Cheque_Date": new Date($scope.fyP_DD_Cheque_Date).toDateString(),
                        "FYP_DD_Cheque_No": $scope.fyP_DD_Cheque_No,
                        "FYP_Date": new Date($scope.FYP_Date).toDateString(),
                        "FYP_Remarks": $scope.FYP_Remarks,
                        "FMG_Id": $scope.fmG_Id,
                    }
                }

                apiService.create("ThirdPartyTransaction/SaveStudentgroupdata", data).
                    then(function (promise) {
                        debugger;
                        if (promise.returnval != null) {
                            if (promise.dulicate != true) {
                                if (promise.returnval == true) {
                                    if ($scope.fyP_Id > 0) {
                                        swal("Record Update Successfully");
                                    } else {
                                        swal("Record Saved Successfully");
                                    }
                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.fyP_Id > 0) {
                                            swal("Record Updation Failed!!!");
                                        }
                                        else {
                                            swal("Record Insertion Failed!!!");
                                        }
                                    }
                                }
                            }
                            else {
                                swal("Receipt No Should Not be Duplicate!!!");
                            }

                        }
                        else {
                            swal("Kindly Contact Administrator!!!");
                        }
                        $state.reload();

                        //if (promise.returnval) {
                        //    swal("Record Saved Successfully");
                        //}
                        //else if (!promise.returnval) {
                        //    swal("Record Not Saved Successfully");
                        //}
                        //else {
                        //    swal("Kindly Contact Admin");
                        //}
                        //$state.reload();
                    })
            }
            else {
                $scope.submitted = true;
            }
        }
        //==============End=============//



        //======Get Single Student Data for Bind into Print Page====// 
        $scope.showmodaldetails = function (fyP_Id, fyptP_Id) {
            $scope.studentdata = [];
            debugger;
            var data = {
                "FYP_Id": fyP_Id,
                "FYPTP_Id": fyptP_Id
            }
            apiService.create("ThirdPartyTransaction/printreceipt", data).
                then(function (promise) {
                    debugger;
                    $scope.studentdata = promise.stdetails;
                    $scope.viewdata = promise.stdetails[0];
                    $scope.mode = $scope.viewdata.fyP_Bank_Or_Cash;
                    $scope.words = $scope.amountinwords(promise.stdetails[0].fyP_Tot_Amount);
                    //$scope.getgroup();
                    //$scope.fmH_Id = promise.studentdata[0].fmH_Id;
                })
        }
        //==============End=============//



        //====Set Year into Calander For Selected Yearlist=========//
        $scope.getdates = function (yr_id, data) {

            if (data != null) {
                debugger;
                console.log(data);
                var name, name1;
                for (var i = 0; i < data.length; i++) {
                    if (i < 4) {
                        if (i == 0) {
                            name = data[i];
                        } else {
                            name += data[i];
                        }
                    }
                    if (i > 4) {
                        if (i == 5) {
                            name1 = data[5];
                        } else {
                            name1 += data[i];
                        }
                    }
                }
                $scope.fromDate = name;
                $scope.toDate = name1;
                $scope.frommon = "";
                $scope.tomon = "";
                $scope.fromDay = "";
                $scope.toDay = "";
                // For Academic From Date
                $scope.minDatemf = new Date(
                    $scope.fromDate,
                    $scope.frommon,
                    $scope.fromDay + 1);

                $scope.maxDatemf = new Date(
                    $scope.toDate,
                    $scope.tomon,
                    $scope.toDay + 365);
                $scope.today = new Date();
            }
        }
        //=========End=========//


        //===========print===========//
        $scope.printData = function () {
            var innerContents = document.getElementById("printmodal").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/FeeTransactionReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        //==============End==============//


        //======Amount Convert into word=========//
        $scope.amountinwords = function convertNumberToWords(atotalc) {
            var words = new Array();
            words[0] = '';
            words[1] = 'One';
            words[2] = 'Two';
            words[3] = 'Three';
            words[4] = 'Four';
            words[5] = 'Five';
            words[6] = 'Six';
            words[7] = 'Seven';
            words[8] = 'Eight';
            words[9] = 'Nine';
            words[10] = 'Ten';
            words[11] = 'Eleven';
            words[12] = 'Twelve';
            words[13] = 'Thirteen';
            words[14] = 'Fourteen';
            words[15] = 'Fifteen';
            words[16] = 'Sixteen';
            words[17] = 'Seventeen';
            words[18] = 'Eighteen';
            words[19] = 'Nineteen';
            words[20] = 'Twenty';
            words[30] = 'Thirty';
            words[40] = 'Forty';
            words[50] = 'Fifty';
            words[60] = 'Sixty';
            words[70] = 'Seventy';
            words[80] = 'Eighty';
            words[90] = 'Ninety';
            atotalc = atotalc.toString();
            var atemp = atotalc.split(".");
            var number = atemp[0].split(",").join("");
            var n_length = number.length;
            var words_string = "";
            if (n_length <= 9) {
                var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
                var received_n_array = new Array();
                for (var i = 0; i < n_length; i++) {
                    received_n_array[i] = number.substr(i, 1);
                }
                for (var i = 9 - n_length, j = 0; i < 9; i++ , j++) {
                    n_array[i] = received_n_array[j];
                }
                for (var i = 0, j = 1; i < 9; i++ , j++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        if (n_array[i] == 1) {
                            n_array[j] = 10 + parseInt(n_array[j]);
                            n_array[i] = 0;
                        }
                    }
                }
                atotalc = "";
                for (var i = 0; i < 9; i++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        atotalc = n_array[i] * 10;
                    } else {
                        atotalc = n_array[i];
                    }
                    if (atotalc != 0) {
                        words_string += words[atotalc] + " ";
                    }
                    if ((i == 1 && atotalc != 0) || (i == 0 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Crores ";
                    }
                    if ((i == 3 && atotalc != 0) || (i == 2 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Lakhs ";
                    }
                    if ((i == 5 && atotalc != 0) || (i == 4 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Thousand ";
                    }
                    if (i == 6 && atotalc != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                        words_string += "Hundred and ";
                    } else if (i == 6 && atotalc != 0) {
                        words_string += "Hundred ";
                    }
                }
                words_string = words_string.split("  ").join(" ");
            }
            return words_string;
        }
        //==============End==============//
    }
})();
