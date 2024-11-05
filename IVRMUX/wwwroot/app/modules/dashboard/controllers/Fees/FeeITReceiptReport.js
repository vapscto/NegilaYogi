(function () {
    'use strict';
    angular
        .module('app')
        .controller('feeitreportController', feeitreportController)

    feeitreportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache', 'blockUI', '$compile']
    function feeitreportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache, blockUI, $compile) {

        $scope.indrpt = false;
        $scope.report = false;
        var institutionid, automanualreceiptnotranum
        $scope.htmldata = "";
        $scope.totalpayable = 0;
        $scope.totalconcession = 0;
        $scope.totalpaid = 0;

        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        $scope.Mi_Id = configsettings[0].mI_Id;
        institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        var headtot = 0;
        var contot = 0;
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        var transactionnumbering = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transactionnumbering.length; i++) {
            if (transactionnumbering.length > 0) {
                if (transactionnumbering[i].imN_Flag == "Online") {
                    $scope.transnumbconfigurationsettingsassign = transactionnumbering[i];
                }
            }
        }

        if (configsettings.length > 0) {
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            receiptformat = configsettings[0].fmC_Receipt_Format;
            mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
        }
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.obj = {};
        $scope.print_flag = false;
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;

            apiService.getURI("feeitreport/getalldetails", pageid).
                then(function (promise) {
                    $scope.onclickloaddataclass();
                    $scope.yearlist = promise.adcyear;
                    $scope.classlist = promise.fillclass;
                    //$scope.fillmasterhead = promise.fillmasterhead;
                })
        }

        //All Receipt Check  
        $scope.binddatagrp = function () {
            $scope.obj.checkallhrd1 = $scope.receiptnoterms.every(function (options) {
                return options.receipt;
            });
        };

        $scope.hrdallcheck1 = function (all) {
            $scope.obj.checkallhrd1 = all;
            var toggleStatus = $scope.obj.checkallhrd1;
            angular.forEach($scope.receiptnoterms, function (options) {
                options.receipt = toggleStatus;
            });
        };  

        //Adding Section 
        $scope.onselectclass = function (clsobj) {
            var data = {
                "ASMAY_Id": clsobj.asmaY_Id,
                "ASMCL_Id": clsobj.asmcL_Id,
            }
            apiService.create("feeitreport/getsection", data).
                then(function (promise) {
                    $scope.sectioncount = promise.fillsection;
                })
        }

        //Added Student
        $scope.onselectsection = function (secobj) {
            $scope.fillstudent = [];
            var data = {
                "ASMAY_Id": secobj.asmaY_Id,
                "ASMCL_Id": secobj.asmcL_Id,
                "AMSC_Id": secobj.asmC_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("feeitreport/getstudent", data).
                then(function (promise) {
                    if (promise.fillstudent != null && promise.fillstudent.length > 0) {
                        $scope.studentlst = promise.fillstudent;
                    }
                })
        };

        //Addedd Receipt
        $scope.onselectstudent = function (std) {
            $scope.valsstd = [];
            $scope.fillmasterhead = [];
            if ($scope.obj.amst_Id > 0) {
                $scope.valsstd.push({
                    AMST_Id: $scope.obj.amst_Id
                });
            }
            else {
                angular.forEach($scope.studentlst, function (dd) {
                    $scope.valsstd.push({
                        AMST_Id: dd.amst_Id
                    });
                });
            }
            var data = {
                "ASMAY_Id": std.asmaY_Id,
                "ASMCL_Id": std.asmcL_Id,
                "AMSC_Id": std.asmC_Id,
                //"FYP_Id": std.fyP_Id,
                "AMST_Id": std.amst_Id,
                studentdata: $scope.valsstd,
            }
            apiService.create("feeitreport/getreceipt", data).
                then(function (promise) {
                    $scope.receiptnoterms = promise.fillterms;
                    $scope.fillmasterhead = promise.fillmasterhead;                  
                })
        }      

        $scope.Clearid = function () {
            $state.reload();
            $scope.loaddata();
        }

        $scope.is_optionrequired_groupterm_grp = function () {
            return !$scope.fillmasterhead.some(function (options) {
                return options.fmH_Id_chk;
            });
        }

        $scope.onclickloaddataclass = function (obj) {
            $scope.submitted = false;
            $scope.obj.asmaY_Id = "";
            $scope.obj.asmcL_Id = "";
            $scope.obj.asmC_Id = "";
            $scope.obj.amst_Id = "";
            $scope.obj.fyp_id = "";

            if ($scope.Selectionrd === 'allr') {
                $scope.head = false;
                $scope.receipt = true;
            }
            else if ($scope.Selectionrd === 'Indi') {
                $scope.receipt = false;
                $scope.head = true;
            }
            else {
                $scope.receipt = false;
                $scope.head = false;
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        ////Report
        $scope.studentlst = [];
        $scope.valsgroup = [];
        $scope.valsreceipt = [];
        $scope.valsstd1 = [];
        $scope.fillmasterhead = [];

        $scope.getreceiptreport = function () {
            angular.forEach($scope.yearlist, function (ty) {
                if (ty.asmaY_Id == $scope.obj.asmaY_Id) {
                    $scope.asmaY_Year = ty.asmaY_Year;
                };
            })

            if ($scope.myForm.$valid) {
                $scope.valsreceipt = [];
                $scope.valshead = [];
                $scope.valsstd = [];
                $scope.valsstd1 = [];

                if ($scope.receiptnoterms != undefined) {
                    for (var u = 0; u < $scope.receiptnoterms.length; u++) {
                        if ($scope.receiptnoterms[u].receipt == true) {
                            $scope.valsreceipt.push($scope.receiptnoterms[u]);
                        }
                    }
                }               

                if ($scope.Selectionrd === "allr") {

                    for (var u = 0; u < $scope.fillmasterhead.length; u++) {
                        if ($scope.fillmasterhead[u].fmH_Ids != "") {
                            $scope.valshead.push($scope.fillmasterhead[u]);
                        }
                    }

                    for (var u = 0; u < $scope.studentlst.length; u++) {
                        if ($scope.studentlst[u].amst_Id != "") {
                            $scope.valsstd.push($scope.studentlst[u]);
                        }
                    }
                    
                    var data = {
                        "ASMAY_Id": $scope.obj.asmaY_Id,
                        "ASMCL_Id": $scope.obj.asmcL_Id,
                        "AMSC_Id": $scope.obj.asmC_Id,                        
                        "reporttype": $scope.Selectionrd,
                        saveheadlst: $scope.valshead,
                        studentdata: $scope.valsstd,
                        receiptlist: $scope.valsreceipt,
                    }

                }
                else if ($scope.Selectionrd === "Indi") {

                    for (var u = 0; u < $scope.fillmasterhead.length; u++) {
                        if ($scope.fillmasterhead[u].fmH_Ids != "") {
                            $scope.valshead.push($scope.fillmasterhead[u]);
                        }
                    }
                                     
                    for (var u = 0; u < $scope.studentlst.length; u++) {
                        if ($scope.studentlst[u].amst_Id == $scope.obj.amst_Id) {
                            $scope.valsstd.push($scope.studentlst[u]);                           
                        }
                    }

                    var data = {
                        "ASMAY_Id": $scope.obj.asmaY_Id,
                        "ASMCL_Id": $scope.obj.asmcL_Id,
                        "AMSC_Id": $scope.obj.asmC_Id,
                        //"AMST_Id": $scope.obj.amst_Id,
                        //"FYP_Id": obj.fyp_id,
                        "reporttype": $scope.Selectionrd,
                        saveheadlst: $scope.valshead,
                        studentdata: $scope.valsstd,
                        receiptlist: $scope.valsreceipt,
                    };
                }
                apiService.create("feeitreport/getreceiptreport", data).then(function (promise) {                    
                    if (promise.receiptdetails != null && promise.receiptdetails.length > 0) {
                        console.log(promise.receiptdetails);
                        $scope.feereceipt = true;
                        var AMST_AdmNo = '';
                        var AMST_RegistrationNo = '';
                        var FYP_DOE = '';
                        var StudentName = '';
                        var AMST_FatherName = '';
                        $scope.tempmainarray = [];
                        $scope.fiestreciept = [];
                        $scope.secondreciept = [];
                        $scope.headarray = [];
                        $scope.receiptdetails = promise.receiptdetails;
                        $scope.valsstd = promise.studentdetaillist;
                        angular.forEach($scope.valsstd, function (dd) {
                            $scope.headarray = [];
                            var tottal = 0;
                            var cls = '';
                            var sec = '';
                            var mode = '';

                            angular.forEach($scope.receiptdetails, function (rr) {

                                if (dd.AMST_Id == rr.AMST_Id) {
                                    $scope.headarray.push({ FMH_FeeName: rr.FMH_FeeName, FYP_Receipt_No: rr.FYP_Receipt_No, Amount: rr.Amount });
                                    AMST_RegistrationNo = rr.AMST_RegistrationNo;
                                    FYP_DOE = rr.FYP_DOE;
                                    AMST_AdmNo = rr.AMST_AdmNo;
                                    StudentName = rr.StudentName;
                                    AMST_FatherName = rr.AMST_FatherName;
                                    tottal += rr.Amount;
                                    mode = rr.FYP_TransactionTypeFlag;

                                    angular.forEach($scope.classlist, function (ff) {
                                        if (rr.ASMCL_Id == ff.asmcL_Id) {
                                            cls = ff.asmcL_ClassName;
                                        }
                                    })
                                    angular.forEach($scope.sectioncount, function (ff1) {
                                        if (rr.ASMS_Id == ff1.amsC_Id) {
                                            sec = ff1.asmc_sectionname;
                                        }
                                    })
                                }
                            })

                            if (tottal > 0) {
                                $scope.words = $scope.amountinwords(tottal);
                                $scope.tempmainarray.push({ AMST_Id: dd.AMST_Id, StudentName: StudentName, AMST_RegistrationNo: AMST_RegistrationNo, AMST_AdmNo: AMST_AdmNo, FYP_DOE: FYP_DOE, stdheadlst: $scope.headarray, total: tottal, words: $scope.words, AMST_FatherName: AMST_FatherName, ASMCL_ClassName: cls, ASMC_SectionName: sec, mode: mode })
                            }
                        }
                        )

                        console.log($scope.tempmainarray)
                        for (var i = 0; i < $scope.tempmainarray.length; i++) {
                            if ((i + 2) % 2 == 0) {
                                $scope.fiestreciept.push($scope.tempmainarray[i]);
                            }
                            else {
                                $scope.secondreciept.push($scope.tempmainarray[i]);
                            }
                        }
                    }
                    else {
                        swal("No Records found.Kindly map Group with heads!!")
                        $scope.grigview1 = false;
                    }
                    
                })
            }
            else {
                $scope.submitted = true;
            }
        }


        //All Head Check
        $scope.allstudentcheck = function () {
            var toggleStatus = $scope.obj.allstdcheck;
            angular.forEach($scope.fillmasterhead, function (itm) {
                itm.fmH_Ids = toggleStatus;
            });
        }

        $scope.binddatahead = function () {
            $scope.obj.allstdcheck = $scope.fillmasterhead.every(function (role) {
                return role.fmH_Ids;
            });
        };

        $scope.ctotalC = function (int) {
            var total = 0;
            angular.forEach($scope.showdetailsreceipt, function (e) {
                total += e.totalcharges;
            });
            return total;
        };

        $scope.printData = function (printmodal) {
            var innerContents = document.getElementById("printmodal").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link href="plugins/bootstrap/css/bootstrap.css" />' +
                '<link href="css/style.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


        $scope.closedata = function () {
            $state.reload();
        }

        $scope.printToCart = function () {

            var pdss = "";

            pdss = 'printrcp'

            var innerContents = document.getElementById(pdss).innerHTML;
            var popupWinindow = window.open('_blank', 'padding-top=50%;');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BArrearReport/itreportpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

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
    }
})();

