(function () {
    'use strict';
    angular
        .module('app')
        .controller('HutchingsChallanController', HutchingsChallanController)
    HutchingsChallanController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function HutchingsChallanController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        if (configsettings.length > 0) {
            $scope.grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            //autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            //receiptformat = configsettings[0].fmC_Receipt_Format;
            //mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
        }
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        if ($scope.grouporterm == "T") {
            $scope.grouportername = "Term Name"
            $scope.term = true;
            $scope.groupterm = false;
        }
        else if ($scope.grouporterm == "G") {
            $scope.grouportername = "Group Name"
            $scope.groupterm = true;
            $scope.term = false;
        }

        $scope.obj = {};

        $scope.clearcheckbox = function (studentlst) {
            $scope.studentid = 0;
            if (studentlst == undefined) {
                $scope.studentid = 0;
            }
            else {
                $scope.studentid = studentlst.amst_Id;
            }

            var data = {
                //"Amst_Id": studid,
                "Amst_Id": $scope.studentid,
                "config": $scope.grouporterm,

            }

            apiService.create("FeeChallanReport/getstudlistgroup", data).
                then(function (promise) {


                    if ($scope.grouporterm == 'T') {
                        var termsdisable = promise.disableterms;

                        if ($scope.termList.length == promise.disableterms.length) {
                            for (var r = 0; r < $scope.termList.length; r++) {
                                if (promise.disableterms[r].netamount <= promise.disableterms[r].paid && $scope.termList[r].fmT_Id == promise.disableterms[r].fmt_id) {
                                    $scope.termList[r].disablepaisterms = true;

                                    $scope.termList[r].Selected2 = false;
                                }
                                else {
                                    $scope.termList[r].disablepaisterms = false;
                                    $scope.termList[r].Selected2 = false;
                                }
                            }
                        }

                        //for challan checking
                        angular.forEach($scope.termList, function (trm1) {
                            angular.forEach(promise.challanterms, function (trm2) {
                                if (trm1.fmT_Id == trm2.fmT_Id) {
                                    trm1.challan_flag = trm2.ischallangenerated;
                                }
                            })
                        })
                    }

                })


        }

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;
            $scope.studentsavedlist = true;
            $scope.enableStudDrpdwn = false;
            apiService.getURI("FeeChallanReport/getInstallments/", pageid).
                then(function (promise) {
                    $scope.feeConfiguration = promise.feeConfiguration;
                    $scope.FMC_GroupOrTermFlg = promise.feeConfiguration[0].fmC_GroupOrTermFlg;
                    $scope.grouplist = promise.grouplist;
                    $scope.termList = promise.termList;
                    $scope.receiptgrid = promise.receiparraydelete;

                    $scope.yearlst = promise.acayear;
                    $scope.ASMAY_Id = $scope.yearlst[0].asmaY_Id;

                    $scope.rolename = angular.lowercase(promise.roleName);

                    if ($scope.rolename == "student") {
                        $scope.enableStudDrpdwn = false;
                    }
                    else {
                        $scope.enableStudDrpdwn = true;
                    }
                    $scope.installmentList = promise.installmentList;

                    $scope.studentlst = promise.admsudentslist;

                    $scope.clearcheckbox();

                })
        }


        $scope.onselectacademic = function (obj) {

            var data = {
                "ASMAY_Id": obj.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeChallanReport/getacademicyear", data).
                then(function (promise) {

                    $scope.studentlst = promise.admsudentslist;
                    $scope.installmentList = promise.installmentList;
                     $scope.receiptgrid = promise.receiparraydelete;

                })
        };


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.Cancel = function () {
            $state.reload();
        }
        $scope.isOptionsRequired1 = function () {

            return !$scope.grouplist.some(function (options) {
                return options.Selected1;
            });
        }
        $scope.isOptionsRequired2 = function () {

            return !$scope.termList.some(function (options) {
                return options.Selected2;
            });
        }


        $scope.showmodaldetails = function (fypid, studid) {
            var data = {
                "Amst_Id": studid,
                "FYP_Id": fypid,
                "config": $scope.grouporterm,
            }
            apiService.create("FeeChallanReport/getChallandetails/", data).
                then(function (promise) {

                    $('#myModal3').modal('show');
                    $scope.stud_det = promise.student_det;
                    $scope.studName = promise.student_det[0].studentName;
                    $scope.amsT_Sex = promise.student_det[0].amsT_Sex;
                    $scope.fatherName = promise.student_det[0].fatherName;
                    $scope.mobNo = promise.student_det[0].mobileNo;
                    $scope.admNo = promise.student_det[0].admNo;
                    $scope.institutionDet = promise.institution_det;
                    $scope.className = promise.className;
                    $scope.tdyDate = promise.datepass;
                    $scope.sectionName = promise.sectionName;
                    $scope.bankDetails = promise.bankDetails;
                    $scope.bankName = promise.bankDetails[0].bank_Name;
                    $scope.address = promise.bankDetails[0].bank_Address;
                    $scope.acc_No = promise.bankDetails[0].acc_No;
                    $scope.acc_Name = promise.bankDetails[0].acC_name;
                    $scope.ifsc_code = promise.bankDetails[0].ifsc + "-" + promise.bankDetails[0].bank_Name;
                    $scope.logo = promise.logo;
                    $scope.institutionName = promise.institution_det[0].mI_Name;
                    $scope.institutionAddress1 = promise.institution_det[0].mI_Address1;
                    $scope.institutionCity = promise.institution_det[0].ivrmmcT_Name;
                    $scope.pincode = promise.institution_det[0].mI_Pincode;
                    $scope.particularsList = promise.particularsList;

                    $scope.period = promise.duration;
                    //var netamt = 0;
                    //for (var i = 0; i < $scope.particularsList.length; i++) {
                    //    netamt = Number(netamt) + Number(promise.particularsList[i].fsS_ToBePaid);
                    //}
                    //$scope.totalamount = netamt;
                    //$scope.amtinwords = $scope.amountinwords($scope.totalamount);
                    //$scope.receiptNo = promise.receiptNo;

                    var netamt = 0;
                    for (var i = 0; i < $scope.particularsList.length; i++) {
                        netamt = Number(netamt) + Number(promise.particularsList[i].fsS_ToBePaid);
                    }

                    $scope.fnltotlamt = netamt;
                    //$scope.amtinwords = $scope.amountinwords($scope.totalamount);
                    $scope.receiptNo = promise.receiptNo;
                    //Added on 23 Feb 2021 for OB issue
                    if (promise.fee_opening_bal != undefined) {
                        if (promise.fee_opening_bal.length > 0) {
                            $scope.openingbalance = promise.fee_opening_bal[0].fsS_PaidAmount - (promise.fee_opening_bal[0].fsS_OBArrearAmount + promise.fee_opening_bal[0].fsS_CurrentYrCharges)

                            if ($scope.opebal > 0) {
                                $scope.openingbalance = promise.fee_opening_bal[0].fsS_PaidAmount - (promise.fee_opening_bal[0].fsS_OBArrearAmount + promise.fee_opening_bal[0].fsS_CurrentYrCharges)
                            }
                            else if ($scope.opebal < 0) {
                                $scope.openingbalance = (promise.fee_opening_bal[0].fsS_OBArrearAmount + promise.fee_opening_bal[0].fsS_CurrentYrCharges) - promise.fee_opening_bal[0].fsS_PaidAmount
                            }
                            else if ($scope.opebal == 0) {
                                $scope.openingbalance = 0
                            }
                        }
                    }
                    //Added on 23 Feb 2021 for OB issue
                    $scope.balance = promise.fillclasflg;
                    $scope.openingbalance = $scope.opebal;

                    $scope.totfine = promise.fillseccls;
                    var a = 0;
                    a = $scope.totfine;
                    a += $scope.fnltotlamt
                    $scope.totalamount = a;
                    $scope.amtinwords = $scope.amountinwords($scope.totalamount);
                    if ($scope.openingbalance > 0) {
                        $scope.particularsList.push({ fmH_FeeName: 'OpeningBalance', fsS_ToBePaid: $scope.openingbalance });
                    }
                    if ($scope.totfine > 0) {
                        $scope.particularsList.push({ fmH_FeeName: 'Fine', fsS_ToBePaid: $scope.totfine });
                    }

                    $scope.test = $scope.particularsList;

                    var iee = 0;
                    if ($scope.balance > 0) {
                        angular.forEach($scope.particularsList, function (test) {
                            if (iee == 0) {
                                //test.fsS_ToBePaid = test.fsS_ToBePaid - $scope.balance;
                                if ($scope.balance > test.fsS_ToBePaid) {
                                    test.fsS_ToBePaid = test.fsS_ToBePaid;
                                }
                                else if (test.fsS_ToBePaid > $scope.balance) {
                                    test.fsS_ToBePaid = test.fsS_ToBePaid - $scope.balance;
                                }
                                iee = iee + 1;
                            }

                        })
                    }


                })
        }




        $scope.submitted = false;
        $scope.selectedGroupList = [];
        $scope.selectedTermList = [];
        $scope.obj = {};
        $scope.generateChallan = function (aaaa) {
            $scope.idofstudent = 0;
            if (aaaa.Amst_Id == undefined) {
                $scope.idofstudent = 0;
            }
            else {
                $scope.idofstudent = aaaa.Amst_Id.amst_Id;
            }
            if ($scope.myForm.$valid) {
                var groupid = "0";
                var termid = "0";
                $scope.selectedGroupList.length = 0;
                $scope.selectedTermList.length = 0;



                for (var i = 0; i < $scope.termList.length; i++) {
                    if ($scope.termList[i].Selected2 == true) {
                        termid = termid + ',' + $scope.termList[i].fmT_Id;
                    }
                }

                for (var i = 0; i < $scope.termList.length; i++) {
                    if ($scope.termList[i].Selected2 == true) {
                        termid = termid + ',' + $scope.termList[i].fmT_Id;
                    }
                }
                
                 if (aaaa.ASMAY_Id == undefined) {
                    aaaa.ASMAY_Id = $scope.ASMAY_Id;
                }
                else {
                    aaaa.ASMAY_Id = aaaa.ASMAY_Id;
                }


                var data = {
                    "Amst_Id": $scope.idofstudent,
                    //  "multiplegrpids": $scope.selectedGroupList,
                    //"selectedTerm": $scope.selectedTermList
                    "multipletrmids": termid,
                    "multiplegrpids": groupid,
                    "config": $scope.grouporterm,
                    "ASMAY_Id": aaaa.ASMAY_Id

                }
                apiService.create("FeeChallanReport/generateChallan/", data).
                    then(function (promise) {

                        var fmH_FeeName = [];
                        var fsS_ToBePaid = [];

                        if (promise.returnval == "nodata") {
                            swal("No Data Available");
                            $('#myModal3').modal('hide');
                            return;
                        }
                        else {
                            $('#myModal3').modal('show');
                            $scope.stud_det = promise.student_det;
                            $scope.studName = promise.student_det[0].studentName;
                            $scope.amsT_Sex = promise.student_det[0].amsT_Sex;
                            $scope.fatherName = promise.student_det[0].fatherName;
                            $scope.mobNo = promise.student_det[0].mobileNo;
                            $scope.admNo = promise.student_det[0].admNo;
                            $scope.institutionDet = promise.institution_det;
                            $scope.className = promise.className;
                            $scope.tdyDate = new Date();
                            $scope.sectionName = promise.sectionName;

                            if (promise.bankDetails.length > 0) {
                                $scope.bankDetails = promise.bankDetails;
                                $scope.bankName = promise.bankDetails[0].bank_Name;
                                $scope.address = promise.bankDetails[0].bank_Address;
                                $scope.acc_No = promise.bankDetails[0].acc_No;
                                $scope.acc_Name = promise.bankDetails[0].acC_name;
                                $scope.ifsc_code = promise.bankDetails[0].ifsc + "-" + promise.bankDetails[0].bank_Name;
                            }

                            $scope.logo = promise.logo;
                            $scope.institutionName = promise.institution_det[0].mI_Name;
                            $scope.institutionAddress1 = promise.institution_det[0].mI_Address1;
                            $scope.institutionCity = promise.institution_det[0].ivrmmcT_Name;
                            $scope.pincode = promise.institution_det[0].mI_Pincode;
                            $scope.particularsList = promise.particularsList;
                            var netamt = 0;
                            for (var i = 0; i < $scope.particularsList.length; i++) {
                                netamt = Number(netamt) + Number(promise.particularsList[i].fsS_ToBePaid);
                            }

                            $scope.fnltotlamt = netamt;
                            //$scope.amtinwords = $scope.amountinwords($scope.totalamount);
                            $scope.receiptNo = promise.receiptNo;
                            $scope.balance = promise.fillclasflg;
                            $scope.totfine = promise.fillseccls;
                            var a = 0;
                            a = $scope.totfine;
                            a += $scope.fnltotlamt
                            $scope.totalamount = a;
                            $scope.amtinwords = $scope.amountinwords($scope.totalamount);
                            if ($scope.balance > 0) {
                                $scope.particularsList.push({ fmH_FeeName: 'OpeningBalance', fsS_ToBePaid: $scope.balance });
                            }
                            if ($scope.totfine > 0) {
                                $scope.particularsList.push({ fmH_FeeName: 'Fine', fsS_ToBePaid: $scope.totfine });
                            }

                            $scope.test = $scope.particularsList;

                            var iee = 0;
                            if ($scope.balance > 0) {
                                angular.forEach($scope.particularsList, function (test) {
                                    if (iee == 0) {
                                        test.fsS_ToBePaid = test.fsS_ToBePaid - $scope.balance;
                                        iee = iee + 1;
                                    }

                                })
                            }
                        }

                    })
            }
            else {
                $scope.submitted = true;
            }
            // $state.reload();
        }

        //Student Search Dropdown
        $scope.searchfilter = function (objj) {

            if (objj.search.length >= '3') {

                $scope.admsudentslist = $scope.studentlst;
                angular.forEach($scope.admsudentslist, function (objectt) {
                    if (objectt.studentName.length > 0) {
                        var string = objectt.studentName;
                        objectt.studentName = string.replace(/  +/g, ' ');
                    }
                })
            }
            else {
                $scope.admsudentslist = [];
            }
        }




        $scope.DeletRecord = function (paymentid, studentid) {


            var data = {
                "FYP_Id": paymentid,
                "Amst_Id": studentid,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }


            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("FeeChallanReport/Deletedetails", data).
                            // apiService.create("FeeStudentTransaction/Deletedetails", data).
                            then(function (promise) {
                                if (promise.returnval == "true") {



                                    swal('Record Deleted Successfully');
                                    $state.reload();
                                }
                                else {
                                    swal('Transaction is not Processed.Kindly contact Administrator!!!!!');
                                }
                                //$state.reload();
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        }



        $scope.amountinwords = function convertNumberToWords(totalc) {
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
            totalc = totalc.toString();
            var atemp = totalc.split(".");
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
                totalc = "";
                for (var i = 0; i < 9; i++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        totalc = n_array[i] * 10;
                    } else {
                        totalc = n_array[i];
                    }
                    if (totalc != 0) {
                        words_string += words[totalc] + " ";
                    }
                    if ((i == 1 && totalc != 0) || (i == 0 && totalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Crores ";
                    }
                    if ((i == 3 && totalc != 0) || (i == 2 && totalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Lakhs ";
                    }
                    if ((i == 5 && totalc != 0) || (i == 4 && totalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Thousand ";
                    }
                    if (i == 6 && totalc != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                        words_string += "Hundred and ";
                    } else if (i == 6 && totalc != 0) {
                        words_string += "Hundred ";
                    }
                }
                words_string = words_string.split("  ").join(" ");
            }
            return words_string;
        }


        var cnt = 0;
        $scope.Ischallangenerated = function (terms, index) {
            if (terms.Selected2) {

                if (terms.challan_flag == 'no') {
                    if (cnt == 0) {
                        var keep_go = true;
                        angular.forEach($scope.termList, function (ty) {
                            if (ty.fmT_Id == terms.fmT_Id) {
                                keep_go = false;
                            }
                            if (keep_go) {
                                if ((ty.disablepaisterms == false || ty.disablepaisterms == undefined) && (ty.challan_flag == 'no' || ty.challan_flag == undefined)) {
                                    if (!ty.Selected2) {

                                        swal("You have select Terms in Order");
                                        $scope.termList[index].Selected2 = false;
                                    }
                                }
                            }
                        })
                    }
                    else {
                        $scope.termList[index].Selected2 = false;
                        swal("You cannot generate a Challan for next term without making payment for previous term");
                    }
                }
                else if (terms.challan_flag == 'yes') {
                    cnt += 1;
                    $scope.termList[index].Selected2 = false;
                    swal("Challan is already generated for selected term");
                }

            }
        }





        $scope.search_flag = false;
        $scope.search123 = "";
        var search_s = "";
        var list_s = [];
        $scope.onselectsearch = function () {
            search_s = $scope.search123;
            list_s = $scope.receiptgrid;
            if (search_s == "" || search_s == undefined) {
                swal("Select Any Field For Search");
                $scope.search_flag = false;
            }
            else {
                $scope.search_flag = true;

                if ($scope.search123 == "5") {
                    // var sub = Number($scope.searchtext);
                    $scope.txt = false;
                    $scope.numbr = true;
                    $scope.dat = false;

                }
                else if ($scope.search123 == "4") {

                    $scope.txt = false;
                    $scope.numbr = false;
                    $scope.dat = true;

                }
                else {
                    $scope.txt = true;
                    $scope.numbr = false;
                    $scope.dat = false;

                }
                $scope.searchtxt = "";
                $scope.searchdat = "";
                $scope.searchnumbr = "";

            }
        }


        $scope.ShowSearch_Report = function () {


            if ($scope.searchtxt != "" || $scope.searchnumbr != "" || $scope.searchdat != "") {
                if ($scope.search123 == "5") {
                    var data = {
                        "searchType": $scope.search123,
                        "searchnumber": $scope.searchnumbr,
                    }
                }
                else if ($scope.search123 == "4") {
                    debugger;
                    var date = new Date($scope.searchdat).toDateString("dd/MM/yyyy");
                    var data = {
                        "searchType": $scope.search123,
                        "searchdate": date,
                    }
                }
                else {

                    var data = {
                        "searchType": $scope.search123,
                        "searchtext": $scope.searchtxt,
                    }

                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeChallanReport/searching", data).
                    then(function (promise) {
                        $scope.receiptgrid = promise.searcharray;
                        $scope.totcountsearch = promise.searcharray.length;

                        if (promise.searcharray == null || promise.searcharray == "") {
                            swal("Record Does Not Exist For Searched Data !!!!!!")
                        }
                    })
            }
            else {
                swal("Data Is Needed For Search ");
            }

        }


        //to print
        $scope.HHChallan = function () {
            var innerContents = document.getElementById("HHChallan").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print"  rel="stylesheet" href="css/print/hutchings/Challan/HHChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

            $state.reload();
        }
    }
})();
