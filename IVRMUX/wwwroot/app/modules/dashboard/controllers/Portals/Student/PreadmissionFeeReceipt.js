(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeReceiptController', FeeReceiptController);

    FeeReceiptController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$compile'];
    function FeeReceiptController($rootScope, $scope, $state, $location, Flash, apiService, $stateParams, $filter, superCache, $window, $compile) {

        var grouporterm, autoreceipt, receiptformat, mergeinstallment, fineapplicable;
        //var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        $scope.disableprint = true;
        $scope.showreceipt = false;
        //var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        $scope.getBase64FromImageUrl = function (URL) {
            var img = new Image();
            img.setAttribute('crossOrigin', 'anonymous');
            img.src = $scope.imgname;
            img.onload = function () {
                var canvas = document.createElement("canvas");
                canvas.width = this.width;
                canvas.height = this.height;
                var ctx = canvas.getContext("2d");
                ctx.drawImage(this, 0, 0);
                $scope.dataURL = canvas.toDataURL("image/png");
            };
        }
        $scope.getBase64FromImageUrl();
        $scope.download = function () {
            $scope.showstyle = true;
            html2canvas(document.getElementById('printmodal2'), {
                onrendered: function (canvas) {
                    var data = canvas.toDataURL();
                    var docDefinition = {
                        content: [{
                            image: data,
                            width: 450,
                            height: 450
                        }]
                    };
                    pdfMake.createPdf(docDefinition).download("FEE RECEIPT.pdf");
                }
            });
            //var options = {};
            //var pdf = new jsPDF('p', 'pt', 'a4');
            //pdf.addHTML($("#test"), options, function () {
            //    pdf.save('pageContent.pdf');
            //});
        };
        //var transactionnumbering = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        //for (var i = 0; i < transactionnumbering.length; i++) {
        //    if (transactionnumbering.length > 0) {
        //        if (transactionnumbering[i].imN_Flag === "Online") {
        //            $scope.transnumbconfigurationsettingsassign = transactionnumbering[i];
        //        }
        //    }
        //}
        //if (configsettings.length > 0) {
        //    grouporterm = configsettings[0].fmC_GroupOrTermFlg;
        //    autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
        //    receiptformat = configsettings[0].fmC_Receipt_Format;
        //    mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
        //}
        //$scope.usrname = localStorage.getItem('username');

        //===================================================== PAGE LOAD
        $scope.bankdetails = true;
        $scope.hide_FeeReceipt = false;
        $scope.hide_FeeReceipt1 = false;

        $scope.loaddata = function () {
            apiService.getDATA("FeeReceipt/getloaddata").
                then(function (promise) {
                    $scope.yearlst = promise.yearlist;
                    swal("If receipt is not generated,then it will be updated within 24 hrs after Successful Payment!!");
                });
        };
        //===================== Academic Year Selection

        $scope.onyearchange123 = function (asmaY_Id) {
            $scope.disableprint = true;
            $scope.showreceipt = false;
            $scope.amsT_Id = "";
            $scope.fyP_Id = "";
            $scope.recnolst = {};
            $scope.stulst = {};
            $scope.showdetailsreceipt = "";
            $scope.getfeereceiptlst = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };
            apiService.create("FeeReceipt/preadmissiongetrecdetails", data).
                then(function (promise) {

                    $scope.stulst = promise.studetailslist;
                    if ($scope.stulst.length==1) {
                        $scope.recnolst = promise.recnolist;
                    }
                    if ($scope.stulst.length === "0") {
                        swal("No Record Found....");
                        $state.reload();
                        $scope.recnolst = "";
                    }
                });
        };
        //========================= Print Receipt
        $scope.bankdet = false;
        $scope.onreceiptchange = function (fypid) {
            $scope.disableprint = false;
            $scope.showreceipt = true;
            $scope.ASMCL_ClassName = "";
            $scope.ASMC_SectionName = "";
            $scope.AMAY_RollNo = "";
            $scope.AMST_AdmNo = "";
            $scope.AMST_FirstName = "";
            $scope.curdate = "";
            $scope.asmaY_Year = "";
            $scope.AMST_FatherName = "";
            $scope.AMST_MotherName = "";
            $scope.receiptno = "";
            $scope.FMCC_ConcessionName = "";
            var totnet = 0, totalbalance = 0, totpaid = 0, totcon = 0, totnetexcludingall = 0;
            var data = {
                "configset": grouporterm,
                "FYP_Id": fypid,
                "minstall": mergeinstallment,
                "ASMAY_ID": $scope.asmaY_Id
            };

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("FeeReceipt/preadmissiongetdetails", data).
                then(function (promise) {

                    $scope.totnetamount = 0;
                    $scope.totpaidamount = 0;
                    $scope.totbalance = 0;

                    $scope.htmldata = promise.htmldata;
                    var totnetamount = 0, totpaidamount = 0,totbalance = 0;
                    if (promise.recnolist.length > 0) {
                        angular.forEach(promise.recnolist, function (e) {
                            totnetamount += e.fsS_NetAmount;
                            totpaidamount += e.fsS_PaidAmount;
                            totbalance += e.fsS_ToBePaid;
                        });
                    }

                    $scope.totnetamount = totnetamount;
                    $scope.totpaidamount = totpaidamount;
                    $scope.totbalance = totbalance;

                    $scope.words = $scope.amountinwords(totpaidamount);

                    //$scope.showdetailsreceipt = promise.recnolist;
                
                 $scope.showdetailsreceipt = promise.recnolist;
                    $scope.shownote = false;
                    if ($scope.showdetailsreceipt !== null && $scope.showdetailsreceipt.length > 0) {
                        if ($scope.showdetailsreceipt[0].fmH_FeeName === "Admission Fee") {
                            $scope.shownote = true;
                        }
                    }

                    $scope.payarr = [];
                    $scope.payarr.push(promise.recnolist[0]);
                    $scope.paymenrgrid = $scope.payarr;

                    $scope.FYP_Bank_Name = promise.recnolist[0].fyP_Bank_Name;
                    $scope.FYP_DD_Cheque_Date = promise.recnolist[0].FYP_DD_Cheque_Date;
                    $scope.FYP_DD_Cheque_No = promise.recnolist[0].FYP_DD_Cheque_No;
                    $scope.transactionid = promise.recnolist[0].transactionid;
                    $scope.FYP_Bank_Or_Cash = promise.recnolist[0].fyP_Bank_Or_Cash;
                    $scope.receiptno = promise.recnolist[0].fyP_Receipt_No;
                    $scope.Paid_Date = $filter('date')(promise.recnolist[0].fyP_Date, "dd-MM-yyyy");

                    $scope.AMST_FirstName = promise.studentfeedetails[0].amsT_FirstName;
                    $scope.ASMCL_ClassName = promise.studentfeedetails[0].asmcL_ClassName;
                    $scope.asmaY_Year = promise.studentfeedetails[0].asmaY_Year;
                    $scope.AMST_AdmNo = promise.studentfeedetails[0].amsT_AdmNo;
                    $scope.AMST_FatherName = promise.studentfeedetails[0].amsT_FatherName;
                    $scope.AMST_MotherName = promise.studentfeedetails[0].amsT_MotherName;
                    $scope.AMST_MobileNo = promise.studentfeedetails[0].amsT_MobileNo;

                    var e1 = angular.element(document.getElementById("test"));
                    $compile(e1.html(promise.htmldata))(($scope));

                });
        };

        $scope.onstudentchange = function (amsT_Id) {
            $scope.disableprint = true;
            $scope.fyP_Id = "";
            $scope.showreceipt = false;
            var data = {
                "AMST_Id": amsT_Id,
                "ASMAY_ID": $scope.asmaY_Id
            };

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            apiService.create("FeeReceipt/getstudetails", data).
                then(function (promise) {
                    if (promise.recnolist.length > 0) {
                        $scope.recnolst = promise.recnolist;
                    }
                });
        };

        //================== PRINT
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
        };

        $scope.printDatahhs = function (printmodal) {
            var innerContents = document.getElementById("printmodal").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print"  rel="stylesheet" href="css/print/hutchings/Challan/HHSRECEIPTPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        //========================= Clear
        $scope.Clear = function () {
            $state.reload();
        };

        //Number to Words
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
    };
})();
