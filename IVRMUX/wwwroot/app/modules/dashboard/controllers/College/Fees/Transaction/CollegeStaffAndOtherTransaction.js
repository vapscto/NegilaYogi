(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeStaffAndOtherTransactionController', CollegeStaffAndOtherTransactionController)

    CollegeStaffAndOtherTransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache', 'blockUI']
    function CollegeStaffAndOtherTransactionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache, blockUI) {

        $scope.studentsavedlist = true;

        $scope.printreceipt = false;

        $scope.printview = true;

        $scope.disableacademicyear = false;
        $scope.disablestaff = false;

        $scope.obj = {};
        //added on 09102017
        $scope.allcheck = false;

        $scope.totcountsearch = 0;
        $scope.totcountsearch_o = 0;
        $scope.disablefine = true;
        $scope.disableconcession = true;
        $scope.disablenetamount = true;
        $scope.disablefsS_CurrentYrCharges = true;
        $scope.disablefsS_TotalToBePaid = true;
        $scope.disablestu = false;

        $scope.rolenamelist = "";

        var institutionid, automanualreceiptnotranum

        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        // $scope.Mi_Id = configsettings[0].mI_Id;
        institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

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

        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        if (grouporterm == "T") {
            $scope.grouportername = "Term Name"
        }
        else if (grouporterm == "G") {
            $scope.grouportername = "Group Name"
        }

        $scope.reloaddata = function () {
            $scope.groupcount = [];
            $scope.grigview1 = false;
            $scope.obj.allgrouporterm = false;
        }

        $scope.search = '';
        $scope.filterOnLocation = function (user1) {
            return angular.lowercase(user1.amsT_FirstName + ' ' + user1.amsT_MiddleName + ' ' + user1.amsT_LastName).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(user1.classname).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(user1.sectionname).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(user1.amsT_AdmNo).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(user1.fyP_Receipt_No).indexOf(angular.lowercase($scope.search)) >= 0 || JSON.stringify(user1.fyP_Tot_Amount).indexOf($scope.search) >= 0 || ($filter('date')(user1.fyP_Date, 'dd-MM-yyyy').indexOf($scope.search) >= 0);

        };
        var temp_grid_staff_list = [];
        var temp_grid_oth_student_list = [];


        $scope.showreceiptno = true;
        $scope.bankdetails = true;

        $scope.totalfee = 0;
        $scope.totalconcession = 0;
        $scope.totalfine = 0;
        $scope.curramount = 0;
        $scope.currbalance = 0;
        $scope.totalwaived = 0;
        $scope.optradio = true;
        $scope.cfg = {};
        $scope.formload = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10

            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = 10

            var pageid = 1;

            apiService.getURI("CollegeStaffAndOtherTransaction/getalldetails", pageid).
                then(function (promise) {
                    $scope.rolenamelist = promise.rolename;
                    // $scope.yearlst = academicyrlst;
                    $scope.yearlst = promise.fillyear;

                    $scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;

                    if (promise.transnumconfig.length > 0) {
                        automanualreceiptnotranum = promise.transnumconfig[0].imN_AutoManualFlag;
                    }

                    //   if (autoreceipt == 1 || promise.transnumconfig[0].imN_AutoManualFlag == "Auto") {

                    if (autoreceipt == 1) {
                        $scope.showreceiptno = false;
                    }
                    else {
                        $scope.showreceiptno = true;
                    }

                    $scope.receiptgrid = promise.receiparraydelete;
                    $scope.FYP_Date = new Date();
                    $scope.FYP_DD_Cheque_Date = new Date();


                    if ($scope.FYP_Bank_Or_Cash == 'C') {
                        $scope.bankdetails = false;
                    }
                    else if ($scope.FYP_Bank_Or_Cash == 'B') {
                        $scope.bankdetails = true;
                    }

                    //configuration settings
                    if (promise.feeconfiglist.length > 0) {
                        grouporterm = promise.feeconfiglist[0].fmC_GroupOrTermFlg;
                        $scope.grouporterm = grouporterm;
                        mergeinstallment = promise.feeconfiglist[0].fmC_RInstallmentsMergeFlag;//added by kiran

                        if (grouporterm == "T") {
                            $scope.grouportername = "Term Name"
                        }
                        else if (grouporterm == "G") {
                            $scope.grouportername = "Group Name"
                        }
                    }
                    //for staff_others
                    $scope.staff_list = promise.stafflist;
                    $scope.student_list = promise.oth_studentlist;
                    $scope.saved_staff_list = promise.staff_paid_details;
                    $scope.saved_others_list = promise.others_paid_details;

                    temp_grid_staff_list = promise.staff_paid_details;
                    temp_grid_oth_student_list = promise.others_paid_details;

                    $scope.get_dates(promise.asmaY_Id, promise.asmaY_Year);


                })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        }




        $scope.searchfilter = function (objj) {

            if (objj.search.length >= '3') {

                $scope.admsudentslist = $scope.staff_list;
                angular.forEach($scope.admsudentslist, function (objectt) {
                    if (objectt.hrmE_EmployeeFirstName.length > 0) {
                        var string = objectt.hrmE_EmployeeFirstName;
                        objectt.hrmE_EmployeeFirstName = string.replace(/  +/g, ' ');
                    }
                })
            }
            else {
                $scope.admsudentslist = [];
            }
        }




        $scope.onselectacademic = function (yearlst) {

            angular.forEach(yearlst, function (op_m) {
                if (op_m.asmaY_Id == $scope.cfg.ASMAY_Id) {
                    $scope.asmaY_Year = op_m.asmaY_Year
                }
            })

            var data = {
                "ASMAY_ID": $scope.cfg.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            //var radioselctionlst = $scope.filterdata
            //var academicyearid = $scope.ASMAY_Id;

            apiService.create("CollegeStaffAndOtherTransaction/getacademicyear", data).
                then(function (promise) {

                    $scope.saved_staff_list = promise.staff_paid_details;
                    $scope.saved_others_list = promise.others_paid_details;

                    $scope.getdates(promise.asmaY_Id, $scope.asmaY_Year);

                })
        };




        $scope.temptermarray = [];
        $scope.groupidss = {};

        $scope.concessionamt = function (totalgrid, index) {
            var count = 0;
            angular.forEach($scope.totalgrid, function (user) {
                if (!!user.isSelected) {
                    count = count + 1
                }
            })

            //  var tobepaidamount = $scope.totalgrid[index].fsS_ToBePaid;
            if (count <= 1) {
                if (Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_ConcessionAmount) + Number(totalgrid[index].fsS_WaivedAmount) <= Number(totalgrid[index].totalamount)) {
                    $scope.totalconcession = Number(totalgrid[index].fsS_ConcessionAmount);
                }
                else if (Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_ConcessionAmount) + Number(totalgrid[index].fsS_WaivedAmount) > Number(totalgrid[index].totalamount)) {
                    swal("Entered amount is greater than Net Amount");
                }
            }

            else if (count > 1) {

                if (Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_ConcessionAmount) + Number(totalgrid[index].fsS_WaivedAmount) <= Number(totalgrid[index].totalamount)) {

                    //$scope.curramount = Number($scope.curramount) - Number(totalgrid[index].fsS_ToBePaid);
                    var interconcessionamt = 0
                    angular.forEach($scope.totalgrid, function (user) {
                        if (!!user.isSelected) {
                            interconcessionamt = Number(interconcessionamt) + Number(user.fsS_ConcessionAmount);
                        }
                    })
                    $scope.totalconcession = interconcessionamt
                }

                else if (Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_ConcessionAmount) + Number(totalgrid[index].fsS_WaivedAmount) > Number(totalgrid[index].totalamount)) {
                    swal("Entered amount is greater than Net Amount");
                }
                //$scope.totalconcession = Number($scope.totalconcession) + Number(totalgrid[index].fsS_ConcessionAmount)

            }
        };

        $scope.fineamt = function (totalgrid, index) {
            var count = 0;
            angular.forEach($scope.totalgrid, function (user) {
                if (!!user.isSelected) {
                    count = count + 1
                }
            })

            //  var tobepaidamount = $scope.totalgrid[index].fsS_ToBePaid;
            if (count <= 1) {

                $scope.totalfine = Number(totalgrid[index].fsS_FineAmount);

                $scope.curramount = $scope.curramount + $scope.totalfine
            }
            else if (count > 1) {

                var interfineamt = 0


                interfineamt = Number(interfineamt) + Number($scope.totalgrid[index].fsS_FineAmount);

                $scope.totalfine = interfineamt + $scope.totalfine

                $scope.curramount = $scope.curramount + interfineamt
                //$scope.totalfine = Number($scope.totalfine) + Number(user.ftp_fine_amt)

            }
        };

        $scope.waivedoffamt = function (totalgrid, index) {
            var count = 0;
            angular.forEach($scope.totalgrid, function (user) {
                if (!!user.isSelected) {
                    count = count + 1
                }
            })

            //  var tobepaidamount = $scope.totalgrid[index].fsS_ToBePaid;
            if (count <= 1) {
                if (Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_ConcessionAmount) + Number(totalgrid[index].fsS_WaivedAmount) <= Number(totalgrid[index].totalamount)) {
                    $scope.totalwaived = Number(totalgrid[index].fsS_WaivedAmount);
                }
                else if (Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_ConcessionAmount) + Number(totalgrid[index].fsS_WaivedAmount) > Number(totalgrid[index].totalamount)) {
                    swal("Entered amount is greater than Net Amount");
                }
            }
            else if (count > 1) {

                $scope.totalwaived = Number($scope.totalwaived) + Number(user.fsS_WaivedAmount)

            }
        };


        $scope.heads1 = [];
        $scope.get_grp_reptno = function (index) {

            $scope.heads1 = [];

            if ($scope.all == true) {
                angular.forEach($scope.totalgrid, function (student) {
                    $scope.heads1.push(student);
                });
            } else {
                angular.forEach($scope.totalgrid, function (student) {
                    if (student.isSelected == true) {
                        $scope.heads1.push(student);
                    }
                });
            }
            var data = {
                "auto_receipt_flag": autoreceipt,
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                temp_head_list: $scope.heads1,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("CollegeStaffAndOtherTransaction/get_grp_reptno", data).
                then(function (promise) {
                    $scope.grpcount = promise.grp_count;
                    $scope.FYP_Receipt_No = promise.auto_FYP_Receipt_No;



                    if (promise.grp_count != 0 && (promise.grp_count != 1 || promise.grp_count > 1)) {
                        swal("Can't Do Transaction For Two Fee Groups At A Time !!!!!!");
                        //if (temp_123 != undefined)
                        //{
                        //    angular.forEach($scope.totalgrid, function (itm) {
                        //        if (temp_123[0].fmH_Id == itm.fmH_Id)
                        //            itm.isSelected = false;
                        //    });
                        //}
                        //MB

                        if ($scope.all == false) {
                            var totalgrid = trp[0].total_grid;
                            var index = trp[0].in_dex;
                            var userdata = trp[0].user_data;
                            angular.forEach($scope.totalgrid, function (itm) {
                                if (itm.fmH_Id == userdata.fmH_Id) {
                                    itm.isSelected = false;
                                }

                            });

                            $scope.all = $scope.totalgrid.every(function (itm) {
                                return itm.isSelected;
                            });

                            if (totalgrid[index].isSelected == true) {

                                $scope.totalconcession = Number($scope.totalconcession) + Number(totalgrid[index].fsS_ConcessionAmount);
                                $scope.totalfine = Number($scope.totalfine) + Number(totalgrid[index].fsS_FineAmount);
                                //$scope.totalfine =  Number(totalgrid[index].fsS_FineAmount);
                                $scope.curramount = Number($scope.curramount) + Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_FineAmount);
                                $scope.totalwaived = Number($scope.totalwaived) + Number(totalgrid[index].fsS_WaivedAmount);
                            }
                            else if (totalgrid[index].isSelected == false) {

                                $scope.totalconcession = Number($scope.totalconcession) - Number(totalgrid[index].fsS_ConcessionAmount);
                                ////$scope.totalfine = Number($scope.totalfine) - Number(totalgrid[index].fsS_FineAmount);
                                $scope.totalfine = Number($scope.totalfine) - Number(totalgrid[index].fsS_FineAmount);
                                // $scope.totalfine = Number($scope.totalfine) - Number(totalgrid[index].fsS_FineAmount);
                                $scope.curramount = Number($scope.curramount) - Number(totalgrid[index].fsS_ToBePaid) - Number(totalgrid[index].fsS_FineAmount);
                                $scope.totalwaived = Number($scope.totalwaived) - Number(totalgrid[index].fsS_WaivedAmount);
                            }

                            trp = [];
                        }
                        else if ($scope.all == true) {
                            //$scope.all = false;
                            $scope.disablefine = true;
                            $scope.disablenetamount = true;
                            $scope.disableconcession = true;

                            var allchkdata = $scope.all;
                            var toggleStatus = $scope.all;

                            $scope.curramount = 0;
                            $scope.totalconcession = 0;
                            $scope.totalfine = 0;
                            $scope.totalwaived = 0;

                            if (allchkdata == true) {

                                for (var index = 0; index < $scope.totalgrid.length; index++) {
                                    $scope.totalconcession = Number($scope.totalconcession) + Number($scope.totalgrid[index].fsS_ConcessionAmount);
                                    $scope.totalfine = Number($scope.totalfine) + Number($scope.totalgrid[index].fsS_FineAmount);
                                    $scope.curramount = Number($scope.curramount) + Number($scope.totalgrid[index].fsS_ToBePaid) + $scope.totalfine;
                                    $scope.totalwaived = Number($scope.totalwaived) + Number($scope.totalgrid[index].fsS_WaivedAmount);
                                }

                            }
                            else {
                                $scope.totalconcession = 0;
                                $scope.totalfine = 0;
                                $scope.curramount = 0;
                                $scope.totalwaived = 0;
                            }

                            angular.forEach($scope.totalgrid, function (itm) {
                                if (itm.fmG_Id == $scope.highestcountgid) {
                                    itm.isSelected = true;
                                }
                                else {
                                    itm.isSelected = false;
                                }

                            });

                            var payableamt = 0, concamt = 0, fneamt = 0;
                            angular.forEach($scope.totalgrid, function (iitm1) {
                                if (iitm1.isSelected == true) {
                                    payableamt = payableamt + iitm1.fsS_ToBePaid;
                                    concamt = concamt + iitm1.fsS_ConcessionAmount;
                                    fneamt = fneamt + iitm1.fsS_FineAmount;
                                }
                                $scope.curramount = payableamt;
                                $scope.totalconcession = concamt;
                                $scope.totalfine = fneamt;

                            });

                        }

                        //MB

                    }
                })
        }

        var trp = [];

        $scope.nextduedate = true;



        $scope.bankdet = false;
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



        $scope.cleardata = function () {

            $state.reload();
            $scope.formload();
        }

        $scope.EditMasterSectvalue = function (employee) {
            $scope.editEmployee = employee.fyghM_Id;
            var orgid = $scope.editEmployee;
            //orgid = 12;
            apiService.getURI("CollegeStaffAndOtherTransaction/Editdetails", orgid).
                then(function (promise) {
                    $scope.addnewbtn = false;
                    $scope.FMG_Id = promise.alldata[0].fmG_Id;
                    $scope.totalgrid.headcount.selected.FMH_Id = promise.alldata[0].fmH_Id;
                    $scope.totalgrid.installmentcount.FMI_Id = promise.alldata[0].fmI_Id;

                    if (promise.alldata[0].fyghM_FineApplicableFlag = 0) {

                        $scope.totalgrid.FYGHM_FineApplicableFlag = true;
                    }
                    if (promise.alldata[0].fyghM_Common_AmountFlag = 0) {

                        $scope.totalgrid.FYGHM_Common_AmountFlag = true;
                    }
                    if (promise.alldata[0].fyghM_ActiveFlag = 0) {

                        $scope.totalgrid.FYGHM_ActiveFlag = true;
                    }
                })
        }

        $scope.filterby = function () {
            var entereddata = $scope.search;

            var data = {
                "FMG_GroupName": $scope.searchthird,
                "FMH_FeeName": $scope.typethird,
                "ASMAY_ID": $scope.cfg.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("CollegeStaffAndOtherTransaction/1", data).
                then(function (promise) {
                    $scope.thirdgrid = promise.alldata;
                    swal("searched Successfully");
                })
        }




        $scope.submitted = false;
        $scope.savedatatrans = [];



        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.closedata = function () {
            $scope.FYP_DD_Cheque_Date = "";
            $scope.FYP_DD_Cheque_No = "";
            $scope.FYP_Bank_Name = "";

            $scope.FYP_Remarks = "";
        }




        $scope.checkforduplicates = function (receiptno) {
            if (receiptno != null && receiptno != undefined && receiptno != "") {
                var data = {
                    "FYP_Receipt_No": receiptno,
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CollegeStaffAndOtherTransaction/feereceiptduplicate", data).
                    then(function (promise) {
                        if (promise.duplicatereceipt.length > 0) {
                            swal("Duplicate Receipt No");
                            //if ($scope.FYP_Id > 0) {
                            //    $scope.FYP_Receipt_No = "";
                            //}
                            //else {
                            //    $scope.FYP_Receipt_No = receiptno;
                            //}
                            $scope.FYP_Receipt_No = "";
                        }
                    })
            }

        }


        $scope.studentlst = [];


        $scope.search_flag = false;
        $scope.search123 = "";
        var search_s = "";
        //var list_s =[];
        $scope.onselectsearch = function () {
            search_s = $scope.search123;
            //list_s = $scope.receiptgrid;
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
            $scope.totcountsearch = 0;
            $scope.saved_staff_list = temp_grid_staff_list;
        }


        $scope.search_flag_o = false;
        $scope.search123_o = "";
        var search_s_o = "";
        //var list_s =[];
        $scope.onselectsearch_o = function () {
            search_s_o = $scope.search123_o;
            //list_s = $scope.receiptgrid;
            if (search_s_o == "" || search_s_o == undefined) {
                swal("Select Any Field For Search");
                $scope.search_flag_o = false;
            }
            else {
                $scope.search_flag_o = true;

                if ($scope.search123_o == "5") {
                    // var sub = Number($scope.searchtext);
                    $scope.txt_o = false;
                    $scope.numbr_o = true;
                    $scope.dat_o = false;

                }
                else if ($scope.search123_o == "4") {

                    $scope.txt_o = false;
                    $scope.numbr_o = false;
                    $scope.dat_o = true;

                }
                else {
                    $scope.txt_o = true;
                    $scope.numbr_o = false;
                    $scope.dat_o = false;

                }
                $scope.searchtxt_o = "";
                $scope.searchdat_o = "";
                $scope.searchnumbr_o = "";

            }
            $scope.totcountsearch_o = 0;
            $scope.saved_others_list = temp_grid_oth_student_list;
        }

        $scope.ShowSearch_Report = function () {

            // var entereddata = $scope.search;
            if ($scope.searchtxt != "" || $scope.searchnumbr != "" || $scope.searchdat != "") {
                if ($scope.search123 == "5") {
                    // var sub = Number($scope.searchtext);
                    //$scope.txt = false;
                    //$scope.numbr = true;
                    //$scope.dat = false;
                    var data = {
                        "searchType": $scope.search123,
                        "searchnumber": $scope.searchnumbr,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id
                        // "searchtext": substring,
                    }
                }
                else if ($scope.search123 == "4") {

                    //$scope.txt = false;
                    //$scope.numbr = false;
                    //$scope.dat = true;
                    var date = new Date($scope.searchdat).toDateString("dd/MM/yyyy");
                    var data = {
                        "searchType": $scope.search123,
                        "searchdate": date,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id
                        // "searchtext": substring,
                    }
                }
                else {
                    //$scope.txt = true;
                    //$scope.numbr = false;
                    //$scope.dat = false;

                    var data = {
                        "searchType": $scope.search123,
                        "searchtext": $scope.searchtxt,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id
                        // "searchtext": substring,
                    }

                }


                // var substring = $scope.searchtext.toLowerCase();
                //var data = {
                //    "searchType": $scope.search123,
                //    "searchtext": $scope.searchtext,
                //   // "searchtext": substring,
                //}

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CollegeStaffAndOtherTransaction/searching_s", data).
                    then(function (promise) {
                        $scope.saved_staff_list = promise.staff_paid_details;
                        $scope.totcountsearch = promise.staff_paid_details.length;

                        if (promise.staff_paid_details == null || promise.staff_paid_details == "") {
                            swal("Record Does Not Exist For Searched Data !!!!!!")
                            $state.reload();
                        }
                        //swal("searched Successfully");
                    })
            }
            else {
                swal("Data Is Needed For Search ");
            }

        }

        $scope.ShowSearch_Report_o = function () {

            // var entereddata = $scope.search;
            if ($scope.searchtxt_o != "" || $scope.searchnumbr_o != "" || $scope.searchdat_o != "") {
                if ($scope.search123_o == "5") {
                    // var sub = Number($scope.searchtext);
                    //$scope.txt = false;
                    //$scope.numbr = true;
                    //$scope.dat = false;
                    var data = {
                        "searchType": $scope.search123_o,
                        "searchnumber": $scope.searchnumbr_o,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id

                        // "searchtext": substring,
                    }
                }
                else if ($scope.search123_o == "4") {

                    //$scope.txt = false;
                    //$scope.numbr = false;
                    //$scope.dat = true;
                    var date_o = new Date($scope.searchdat_o).toDateString("dd/MM/yyyy");
                    var data = {
                        "searchType": $scope.search123_o,
                        "searchdate": date_o,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id
                        // "searchtext": substring,
                    }
                }
                else {
                    //$scope.txt = true;
                    //$scope.numbr = false;
                    //$scope.dat = false;

                    var data = {
                        "searchType": $scope.search123_o,
                        "searchtext": $scope.searchtxt_o,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id
                        // "searchtext": substring,
                    }

                }


                // var substring = $scope.searchtext.toLowerCase();
                //var data = {
                //    "searchType": $scope.search123,
                //    "searchtext": $scope.searchtext,
                //   // "searchtext": substring,
                //}

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CollegeStaffAndOtherTransaction/searching_o", data).
                    then(function (promise) {
                        $scope.saved_others_list = promise.others_paid_details;
                        $scope.totcountsearch_o = promise.others_paid_details.length;

                        if (promise.others_paid_details == null || promise.others_paid_details == "") {
                            swal("Record Does Not Exist For Searched Data !!!!!!")
                            $state.reload();
                        }
                        //swal("searched Successfully");
                    })
            }
            else {
                swal("Data Is Needed For Search ");
            }

        }


        $scope.clearsearch = function () {
            $scope.search123 = "";
            // $scope.onselectsearch();
            $scope.search_flag = false;
            //  $scope.receiptgrid = "";

            //$state.reload();

            $scope.searchtxt = "";
            $scope.searchnumbr = "";
            $scope.searchdat = "";
            $scope.totcountsearch = 0;
            $scope.saved_staff_list = temp_grid_staff_list;

        }

        $scope.clearsearch_o = function () {
            $scope.search123_o = "";
            // $scope.onselectsearch();
            $scope.search_flag_o = false;
            //  $scope.receiptgrid = "";

            //$state.reload();

            $scope.searchtxt_o = "";
            $scope.searchnumbr_o = "";
            $scope.searchdat_o = "";
            $scope.totcountsearch_o = 0;
            $scope.saved_others_list = temp_grid_oth_student_list;
        }


        $scope.alltermchk = false;

        $scope.updateshowlabel = false;
        $scope.showstudentname = true;

        $scope.diablemodeofpayment = false;

        $scope.edittransaction = function (fypid, amstid) {
            $scope.FYP_Id = fypid;
            $scope.FMOST_Id = amstid;

            var data = {
                "FYP_Id": fypid,
                "Amst_Id": amstid
            }

            apiService.create("CollegeStaffAndOtherTransaction/edittransaction", data).
                then(function (promise) {

                    $scope.alltermchk = true;

                    $scope.allcheck = true;
                    $scope.grigview1 = true;

                    $scope.updateshowlabel = true;
                    $scope.showstudentname = false;

                    $scope.diablemodeofpayment = true;

                    $scope.totalgrid = promise.receiparraydeleteall;



                    for (var s = 0; s < $scope.totalgrid.length; s++) {
                        $scope.totalgrid[s].isSelected = true;
                    }

                    $scope.groupcount = promise.fillmastergroup;

                    for (var r = 0; r < $scope.groupcount.length; r++) {
                        $scope.groupcount[r].disablepaisterms = true;
                    }

                    for (var p = 0; p < promise.disableterms.length; p++) {
                        for (var q = 0; q < $scope.groupcount.length; q++) {
                            if ($scope.groupcount[q].fmG_Id == promise.disableterms[p].fmG_Id) {
                                $scope.groupcount[q].selected = true;
                            }
                        }
                    }

                    $scope.studentlst = promise.receiparraydelete
                    $scope.amsT_FirstName = promise.receiparraydelete[0].amsT_FirstName + ' ' + promise.receiparraydelete[0].amsT_MiddleName + ' ' +
                        promise.receiparraydelete[0].amsT_LastName

                    $scope.updateshowlabel = true;
                    $scope.updatename = $scope.amsT_FirstName

                    $scope.studidd = promise.receiparraydelete[0].amst_Id

                    $scope.amsT_MiddleName = promise.receiparraydelete[0].amsT_MiddleName;
                    $scope.amsT_LastName = promise.receiparraydelete[0].amsT_LastName

                    $scope.amsT_AdmNo = promise.receiparraydelete[0].amsT_AdmNo
                    $scope.amsT_RegistrationNo = promise.receiparraydelete[0].amsT_RegistrationNo
                    $scope.amaY_RollNo = promise.receiparraydelete[0].amaY_RollNo;

                    $scope.classname = promise.receiparraydelete[0].classname
                    $scope.sectionname = promise.receiparraydelete[0].sectionname;

                    $scope.amsT_mobile = promise.receiparraydelete[0].amst_mobile;

                    $scope.fathername = promise.receiparraydelete[0].fathername;
                    $scope.studentdob = promise.receiparraydelete[0].studentdob;

                    //$scope.FYP_Date = $filter('date')(promise.receiparraydelete[0].fyP_Date, "dd-MM-yyyy");
                    $scope.FYP_Date = new Date(promise.receiparraydelete[0].fyP_Date);

                    if (promise.receiparraydelete[0].fyP_Bank_Or_Cash == 'C') {
                        $scope.FYP_Bank_Or_Cash = 'C';
                        $scope.bankdetails = false;
                    }
                    else if (promise.receiparraydelete[0].fyP_Bank_Or_Cash == 'B') {
                        $scope.FYP_Bank_Or_Cash = 'B';
                        $scope.bankdetails = true;
                    }
                    else if (promise.receiparraydelete[0].fyP_Bank_Or_Cash == 'S') {
                        $scope.FYP_Bank_Or_Cash = 'S';
                        $scope.bankdetails = true;
                    }
                    else if (promise.receiparraydelete[0].fyP_Bank_Or_Cash == 'R') {
                        $scope.FYP_Bank_Or_Cash = 'R';
                        $scope.bankdetails = true;
                    }

                    // $scope.FYP_DD_Cheque_Date = $filter('date')(promise.receiparraydelete[0].fyP_DD_Cheque_Date, "dd-MM-yyyy");
                    $scope.FYP_DD_Cheque_Date = new Date(promise.receiparraydelete[0].fyP_DD_Cheque_Date);
                    $scope.FYP_DD_Cheque_No = promise.receiparraydelete[0].fyP_DD_Cheque_No
                    $scope.FYP_Bank_Name = promise.receiparraydelete[0].fyP_Bank_Name

                    $scope.totalfee = promise.receiparraydelete[0].fyP_Tot_Amount
                    $scope.curramount = promise.receiparraydelete[0].fyP_Tot_Amount

                    if (autoreceipt == 1 || promise.transnumconfig[0].imN_AutoManualFlag == "Auto") {
                        $scope.showreceiptno = false;
                    }
                    else {
                        $scope.showreceiptno = true;
                        $scope.FYP_Receipt_No = promise.receiparraydelete[0].fyP_Receipt_No
                    }
                }
                )
        }

        //for staff_others

        $scope.edittransactionstaff = function (fypid, hrmeid) {
            ////$scope.FYP_Id = fypid;
            ////$scope.FMOST_Id = amstid;


            var data = {
                "FYP_Id": fypid,
                "HRME_Id": hrmeid,
                "Grp_Term_flg": grouporterm,
                "Stf_Others_flg": $scope.filterdata,
                "ASMAY_ID": $scope.cfg.ASMAY_Id
            }

            apiService.create("CollegeStaffAndOtherTransaction/edittransactionstaff", data).
                then(function (promise) {

                    $scope.alltermchk = true;

                    $scope.allcheck = true;
                    $scope.grigview1 = true;

                    $scope.updateshowlabel = true;
                    $scope.showstudentname = false;

                    $scope.diablemodeofpayment = true;

                    $scope.totalgrid = promise.receiparraydeleteall;



                    for (var s = 0; s < $scope.totalgrid.length; s++) {
                        $scope.totalgrid[s].isSelected = true;
                    }

                    if (grouporterm == "T") {
                        $scope.term_list = promise.fillmastergroup;

                        for (var r = 0; r < $scope.term_list.length; r++) {
                            $scope.term_list[r].t_disable = true;
                        }

                        for (var p = 0; p < promise.disableterms.length; p++) {
                            for (var q = 0; q < $scope.term_list.length; q++) {
                                if ($scope.term_list[q].fmT_Id == promise.disableterms[p].fmG_Id) {
                                    $scope.term_list[q].selected = true;
                                }
                            }
                        }
                    }
                    else if (grouporterm == "G") {
                        $scope.group_list = promise.fillmastergroup;

                        for (var r = 0; r < $scope.group_list.length; r++) {
                            $scope.group_list[r].t_disable = true;
                        }

                        for (var p = 0; p < promise.disableterms.length; p++) {
                            for (var q = 0; q < $scope.group_list.length; q++) {
                                $scope.group_list[q].g_disable = true;
                                if ($scope.group_list[q].fmG_Id == promise.disableterms[p].fmG_Id) {
                                    $scope.group_list[q].selected = true;

                                }
                            }
                        }
                    }


                    $scope.studentlst = promise.receiparraydelete

                    if ($scope.filterdata == "Staff") {
                        $scope.HRME_EmployeeFirstName = promise.receiparraydelete[0].hrmE_EmployeeFirstName + ' ' + promise.receiparraydelete[0].hrmE_EmployeeMiddleName + ' ' +
                            promise.receiparraydelete[0].hrmE_EmployeeLastName

                        $scope.updateshowlabel = true;

                        //$scope.HRME_Id = promise.receiparraydelete[0].hrmE_Id

                        $scope.HRME_Id = promise.receiparraydelete[0];

                        $scope.accchange = true;

                        $scope.disablestaff = true;
                        $scope.disableacademicyear = true;
                        $scope.all_g_disable = true;

                        $scope.Head_Instl_list = promise.receiparraydeleteall;

                        $scope.HRME_EmployeeCode = promise.receiparraydelete[0].hrmE_EmployeeCode

                        $scope.HRMD_DepartmentName = promise.receiparraydelete[0].hrmD_DepartmentName
                        $scope.HRMDES_DesignationName = promise.receiparraydelete[0].hrmdeS_DesignationName;
                    }

                    if ($scope.filterdata == "Others") {
                        $scope.FMOST_StudentName = promise.receiparraydelete[0].hrmE_EmployeeFirstName + ' ' + promise.receiparraydelete[0].hrmE_EmployeeMiddleName + ' ' +
                            promise.receiparraydelete[0].hrmE_EmployeeLastName

                        $scope.updateshowlabel = true;

                        $scope.disablestu = true;

                        $scope.FMOST_Id = promise.receiparraydelete[0].hrmE_Id

                        $scope.disablestaff = true;
                        $scope.disableacademicyear = true;
                        $scope.all_g_disable = true;

                        $scope.Head_Instl_list = promise.receiparraydeleteall;

                        $scope.FMOST_StudentMobileNo = promise.receiparraydelete[0].amst_mobile

                        $scope.FMOST_StudentEmailId = promise.receiparraydelete[0].fmosT_StudentEmailId
                        //$scope.HRMDES_DesignationName = promise.receiparraydelete[0].hrmdeS_DesignationName;
                    }


                    $scope.FYP_Date = new Date(promise.receiparraydelete[0].fyP_Date);

                    $scope.FYP_Remarks = promise.receiparraydelete[0].fyP_Remarks

                    if (promise.receiparraydelete[0].fyP_Bank_Or_Cash == 'C') {
                        $scope.FYP_Bank_Or_Cash = 'C';
                        $scope.bankdetails = false;
                    }
                    else if (promise.receiparraydelete[0].fyP_Bank_Or_Cash == 'B') {
                        $scope.FYP_Bank_Or_Cash = 'B';
                        $scope.bankdetails = true;
                    }
                    else if (promise.receiparraydelete[0].fyP_Bank_Or_Cash == 'S') {
                        $scope.FYP_Bank_Or_Cash = 'S';
                        $scope.bankdetails = true;
                    }
                    else if (promise.receiparraydelete[0].fyP_Bank_Or_Cash == 'R') {
                        $scope.FYP_Bank_Or_Cash = 'R';
                        $scope.bankdetails = true;
                    }

                    // $scope.FYP_DD_Cheque_Date = $filter('date')(promise.receiparraydelete[0].fyP_DD_Cheque_Date, "dd-MM-yyyy");
                    $scope.FYP_DD_Cheque_Date = new Date(promise.receiparraydelete[0].fyP_DD_Cheque_Date);
                    $scope.FYP_DD_Cheque_No = promise.receiparraydelete[0].fyP_DD_Cheque_No
                    $scope.FYP_Bank_Name = promise.receiparraydelete[0].fyP_Bank_Name

                    $scope.totalfee = promise.receiparraydelete[0].fyP_Tot_Amount
                    $scope.FYP_Tot_Amount = promise.receiparraydelete[0].fyP_Tot_Amount

                    if (autoreceipt == 1 || promise.transnumconfig[0].imN_AutoManualFlag == "Auto") {
                        $scope.showreceiptno = false;
                    }
                    else {
                        $scope.showreceiptno = true;
                        $scope.FYP_Receipt_No = promise.receiparraydelete[0].fyP_Receipt_No
                    }
                }
                )
        }



        $scope.select_emp = function (stf_id) {
            if (stf_id != undefined && stf_id != "") {
                $scope.clear_values();
                var data = {
                    "Grp_Term_flg": grouporterm,
                    // "HRME_Id": $scope.HRME_Id, 
                    "HRME_Id": stf_id.hrmE_Id,
                    "Stf_Others_flg": $scope.filterdata,
                    "ASMAY_ID": $scope.cfg.ASMAY_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CollegeStaffAndOtherTransaction/select_emp", data).
                    then(function (promise) {

                        if (promise.showstaticticsdetails.length > 0) {
                            $scope.staffviewdetails = promise.showstaticticsdetails;
                        }


                        if (grouporterm == 'T') {
                            if (promise.termlist.length > 0) {

                                var temp_termlist = [];
                                for (var x = 0; x < promise.termlist.length; x++) {
                                    var netamount = 0;
                                    var paidamount = 0;
                                    angular.forEach(promise.termlist, function (opw) {
                                        if (opw.fmT_Id == promise.termlist[x].fmT_Id) {
                                            netamount += opw.netAmount;
                                            paidamount += opw.paidAmount;
                                        }
                                    })

                                    if (temp_termlist.length == 0) {
                                        temp_termlist.push({ fmT_Id: promise.termlist[x].fmT_Id, fmT_Name: promise.termlist[x].fmT_Name, fmT_Order: promise.termlist[x].fmT_Order, netAmount: netamount, paidAmount: paidamount });
                                    }
                                    else if (temp_termlist.length > 0) {
                                        var already_cnt = 0;
                                        angular.forEach(temp_termlist, function (opgtf) {
                                            if (opgtf.fmT_Id == promise.termlist[x].fmT_Id) {
                                                already_cnt += 1;
                                            }
                                        })
                                        if (already_cnt == 0) {
                                            temp_termlist.push({ fmT_Id: promise.termlist[x].fmT_Id, fmT_Name: promise.termlist[x].fmT_Name, fmT_Order: promise.termlist[x].fmT_Order, netAmount: netamount, paidAmount: paidamount });
                                        }

                                    }
                                }

                                promise.termlist = temp_termlist;

                                angular.forEach(promise.termlist, function (t) {
                                    if (t.netAmount <= t.paidAmount) {
                                        t.t_disable = true;
                                    }
                                    else {
                                        t.t_disable = false;
                                    }
                                })
                                $scope.all_t_disable = promise.termlist.every(function (options) {
                                    return options.t_disable;
                                });
                                $scope.term_list = promise.termlist;


                                $scope.group_list = [];
                            }
                            else {
                                // swal("Kindly map Staff with group (or) Staff Paid all");
                                swal("Kindly Map Staff With Group");
                            }
                        }
                        else if (grouporterm == 'G') {
                            if (promise.grouplist.length > 0) {

                                var temp_grouplist = [];
                                for (var x = 0; x < promise.grouplist.length; x++) {
                                    var netamount = 0;
                                    var paidamount = 0;
                                    angular.forEach(promise.grouplist, function (opw) {
                                        if (opw.fmG_Id == promise.grouplist[x].fmG_Id) {
                                            netamount += opw.netAmount;
                                            paidamount += opw.paidAmount;
                                        }
                                    })

                                    if (temp_grouplist.length == 0) {
                                        temp_grouplist.push({ fmG_Id: promise.grouplist[x].fmG_Id, fmG_GroupName: promise.grouplist[x].fmG_GroupName, netAmount: netamount, paidAmount: paidamount });
                                    }
                                    else if (temp_grouplist.length > 0) {
                                        var already_cnt = 0;
                                        angular.forEach(temp_grouplist, function (opgtf) {
                                            if (opgtf.fmG_Id == promise.grouplist[x].fmG_Id) {
                                                already_cnt += 1;
                                            }
                                        })
                                        if (already_cnt == 0) {
                                            temp_grouplist.push({ fmG_Id: promise.grouplist[x].fmG_Id, fmG_GroupName: promise.grouplist[x].fmG_GroupName, netAmount: netamount, paidAmount: paidamount });
                                        }

                                    }
                                }

                                promise.grouplist = temp_grouplist;

                                angular.forEach(promise.grouplist, function (g) {
                                    if (g.netAmount <= g.paidAmount) {
                                        g.g_disable = true;
                                    }
                                    else {
                                        g.g_disable = false;
                                    }
                                })
                                $scope.all_g_disable = promise.grouplist.every(function (options) {
                                    return options.g_disable;
                                });
                                $scope.group_list = promise.grouplist;
                                $scope.term_list = [];

                            }
                            else {
                                //swal("Kindly map Staff with group (or) Staff Paid all");
                                swal("Kindly Map Staff With Group");
                            }
                        }
                        $scope.HRME_EmployeeCode = promise.stafflist[0].hrmE_EmployeeCode;
                        $scope.HRME_EmployeeFirstName = promise.stafflist[0].hrmE_EmployeeFirstName;
                        $scope.HRMD_DepartmentName = promise.stafflist[0].hrmD_DepartmentName;
                        $scope.HRMDES_DesignationName = promise.stafflist[0].hrmdeS_DesignationName;

                        //$scope.FMOST_StudentName = "";
                        //$scope.FMOST_StudentMobileNo = "";
                        //$scope.FMOST_StudentEmailId = "";
                        // $scope.clear_values();
                    })
            }
        };
        $scope.select_student = function (stu_id) {
            if (stu_id != undefined && stu_id != "") {
                $scope.clear_values();
                var data = {
                    "Grp_Term_flg": grouporterm,
                    "FMOST_Id": $scope.FMOST_Id,
                    "ASMAY_Id": $scope.cfg.ASMAY_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CollegeStaffAndOtherTransaction/select_student", data).
                    then(function (promise) {

                        if (promise.showstaticticsdetails.length > 0) {
                            $scope.staffviewdetails = promise.showstaticticsdetails;
                        }

                        if (grouporterm == 'T') {
                            if (promise.termlist.length > 0) {

                                var temp_termlist = [];
                                for (var x = 0; x < promise.termlist.length; x++) {
                                    var netamount = 0;
                                    var paidamount = 0;
                                    angular.forEach(promise.termlist, function (opw) {
                                        if (opw.fmT_Id == promise.termlist[x].fmT_Id) {
                                            netamount += opw.netAmount;
                                            paidamount += opw.paidAmount;
                                        }
                                    })

                                    if (temp_termlist.length == 0) {
                                        temp_termlist.push({ fmT_Id: promise.termlist[x].fmT_Id, fmT_Name: promise.termlist[x].fmT_Name, fmT_Order: promise.termlist[x].fmT_Order, netAmount: netamount, paidAmount: paidamount });
                                    }
                                    else if (temp_termlist.length > 0) {
                                        var already_cnt = 0;
                                        angular.forEach(temp_termlist, function (opgtf) {
                                            if (opgtf.fmT_Id == promise.termlist[x].fmT_Id) {
                                                already_cnt += 1;
                                            }
                                        })
                                        if (already_cnt == 0) {
                                            temp_termlist.push({ fmT_Id: promise.termlist[x].fmT_Id, fmT_Name: promise.termlist[x].fmT_Name, fmT_Order: promise.termlist[x].fmT_Order, netAmount: netamount, paidAmount: paidamount });
                                        }

                                    }
                                }

                                promise.termlist = temp_termlist;

                                angular.forEach(promise.termlist, function (t) {
                                    if (t.netAmount <= t.paidAmount) {
                                        t.t_disable = true;
                                    }
                                    else {
                                        t.t_disable = false;
                                    }
                                })
                                $scope.all_t_disable = promise.termlist.every(function (options) {
                                    return options.t_disable;
                                });
                                $scope.term_list = promise.termlist;


                                $scope.group_list = [];
                            }
                            else {
                                // swal("Kindly map Student with group (or) Student Paid all");
                                swal("Kindly Map Student With Group");
                            }
                        }
                        else if (grouporterm == 'G') {
                            if (promise.grouplist.length > 0) {

                                var temp_grouplist = [];
                                for (var x = 0; x < promise.grouplist.length; x++) {
                                    var netamount = 0;
                                    var paidamount = 0;
                                    angular.forEach(promise.grouplist, function (opw) {
                                        if (opw.fmG_Id == promise.grouplist[x].fmG_Id) {
                                            netamount += opw.netAmount;
                                            paidamount += opw.paidAmount;
                                        }
                                    })

                                    if (temp_grouplist.length == 0) {
                                        temp_grouplist.push({ fmG_Id: promise.grouplist[x].fmG_Id, fmG_GroupName: promise.grouplist[x].fmG_GroupName, netAmount: netamount, paidAmount: paidamount });
                                    }
                                    else if (temp_grouplist.length > 0) {
                                        var already_cnt = 0;
                                        angular.forEach(temp_grouplist, function (opgtf) {
                                            if (opgtf.fmG_Id == promise.grouplist[x].fmG_Id) {
                                                already_cnt += 1;
                                            }
                                        })
                                        if (already_cnt == 0) {
                                            temp_grouplist.push({ fmG_Id: promise.grouplist[x].fmG_Id, fmG_GroupName: promise.grouplist[x].fmG_GroupName, netAmount: netamount, paidAmount: paidamount });
                                        }

                                    }
                                }

                                promise.grouplist = temp_grouplist;

                                angular.forEach(promise.grouplist, function (g) {
                                    if (g.netamount <= g.paidamount) {
                                        g.g_disable = true;
                                    }
                                    else {
                                        g.g_disable = false;
                                    }
                                })
                                $scope.all_g_disable = promise.grouplist.every(function (options) {
                                    return options.g_disable;
                                });
                                $scope.group_list = promise.grouplist;
                                $scope.term_list = [];

                            }
                            else {
                                // swal("Kindly map Student with group (or) Student Paid all");
                                swal("Kindly Map Student With Group");
                            }
                        }
                        //$scope.HRME_EmployeeCode = "";
                        //$scope.HRME_EmployeeFirstName = "";
                        //$scope.HRMD_DepartmentName = "";
                        //$scope.HRMDES_DesignationName = "";

                        $scope.FMOST_StudentName = promise.oth_studentlist[0].fmosT_StudentName;
                        $scope.FMOST_StudentMobileNo = promise.oth_studentlist[0].fmosT_StudentMobileNo;
                        $scope.FMOST_StudentEmailId = promise.oth_studentlist[0].fmosT_StudentEmailId;

                        //$scope.clear_values();

                    })
            }
        };
        $scope.term_list = [];
        $scope.group_list = [];
        $scope.FYP_Tot_Amount = 0;
        $scope.FYP_Tot_Fine_Amt = 0;
        $scope.FYP_Tot_Concession_Amt = 0;
        $scope.clear_values = function () {
            if ($scope.filterdata == 'Staff') {
                $scope.FMOST_Id = "";
                $scope.FMOST_StudentName = "";
                $scope.FMOST_StudentMobileNo = "";
                $scope.FMOST_StudentEmailId = "";
            }
            else if ($scope.filterdata == 'Others') {
                $scope.HRME_Id = "";
                $scope.HRME_EmployeeCode = "";
                $scope.HRME_EmployeeFirstName = "";
                $scope.HRMD_DepartmentName = "";
                $scope.HRMDES_DesignationName = "";
            }

            $scope.staffviewdetails = [];

            $scope.FYP_Bank_Or_Cash = 'B';
            $scope.term_list = [];
            $scope.group_list = [];
            $scope.usercheck_t = false;
            $scope.usercheck_g = false;
            $scope.Head_Instl_list = [];
            $scope.grigview1 = false;
            $scope.totalfee = 0;
            $scope.FYP_Tot_Concession_Amt = 0;
            $scope.FYP_Tot_Fine_Amt = 0;
            $scope.FYP_Tot_Amount = 0;
            $scope.currbalance = 0;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }
        $scope.all_check_terms = function () {

            var toggleStatus = $scope.usercheck_t;
            angular.forEach($scope.term_list, function (itm) {
                if (!itm.t_disable) {
                    itm.selected = toggleStatus;
                }
                //itm.selected = toggleStatus;
            });
            $scope.togchkbx_group_term();
        }
        //$scope.togchkbx_term = function () {
        //    
        //    $scope.usercheck_t = $scope.term_list.every(function (options) {
        //        return options.selected;
        //    });
        //}
        $scope.all_check_groups = function () {

            var toggleStatus = $scope.usercheck_g;
            angular.forEach($scope.group_list, function (itm) {
                if (!itm.g_disable) {
                    itm.selected = toggleStatus;
                }
                // itm.selected = toggleStatus;
            });
            $scope.togchkbx_group_term();
        }
        //$scope.togchkbx_group = function () {
        //    
        //    $scope.usercheck_g = $scope.group_list.every(function (options) {
        //        return options.selected;
        //    });
        //}
        $scope.togchkbx_group_term = function (option) {

            var terms_OR_groups = "";
            $scope.all = false;
            $scope.terms_groups = [];

            if (grouporterm == 'T') {
                terms_OR_groups = "Terms";
                $scope.usercheck_t = $scope.term_list.every(function (options) {
                    return options.selected;
                });
                angular.forEach($scope.term_list, function (trm_grp) {
                    if (trm_grp.selected) {
                        $scope.terms_groups.push(trm_grp.fmT_Id);
                    }
                })

            }
            else if (grouporterm == 'G') {
                terms_OR_groups = "Groups";
                $scope.usercheck_g = $scope.group_list.every(function (options) {
                    return options.selected;
                });
                angular.forEach($scope.group_list, function (trm_grp) {
                    if (trm_grp.selected) {
                        $scope.terms_groups.push(trm_grp.fmG_Id);
                    }
                })

            }

            if ($scope.filterdata == 'Staff') {
                var data = {
                    //"HRME_Id": $scope.HRME_Id,
                    "HRME_Id": $scope.HRME_Id.hrmE_Id,
                    terms_groups: $scope.terms_groups,
                    "Grp_Term_flg": grouporterm,
                    "Stf_Others_flg": $scope.filterdata,
                    "ASMAY_Id": $scope.cfg.ASMAY_Id

                }
            }
            else if ($scope.filterdata == 'Others') {
                var data = {
                    "FMOST_Id": $scope.FMOST_Id,
                    terms_groups: $scope.terms_groups,
                    "Grp_Term_flg": grouporterm,
                    "Stf_Others_flg": $scope.filterdata,
                    "ASMAY_Id": $scope.cfg.ASMAY_Id
                }
            }


            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("CollegeStaffAndOtherTransaction/getgroupmappedheadsnew_st", data).
                then(function (promise) {

                    if (promise.mapped_hds_ins.length > 0) {
                        $scope.grigview1 = true;

                        //added on 09102017
                        $scope.Head_Instl_list = promise.mapped_hds_ins;


                        //hema


                        //hema
                        var addfinetonetamount, addnetamount, totalpayableamount;
                        // $scope.totalgrid = $scope.temptermarray;

                        for (var i = 0; i < $scope.Head_Instl_list.length; i++) {
                            addfinetonetamount = $scope.Head_Instl_list[i].fineAmount;
                            addnetamount = $scope.Head_Instl_list[i].totalCharges;
                            if (Number(addfinetonetamount) > 0) {
                                $scope.Head_Instl_list[i].netAmount = Number(addfinetonetamount) + Number(addnetamount);
                            }
                            else {
                                $scope.Head_Instl_list[i].netAmount = Number(addnetamount);
                            }
                        }

                        var cur_yr_chgs = 0;
                        var to_be_paid = 0;


                        angular.forEach($scope.Head_Instl_list, function (value, key) {
                            //  if (value.amst_Id == 0) {
                            cur_yr_chgs = cur_yr_chgs + value.currentYrCharges;
                            to_be_paid = to_be_paid + value.toBePaid;
                            // }
                        })



                        $scope.totalfee = cur_yr_chgs;
                        $scope.currbalance = to_be_paid;

                    }
                    else {

                        //swal(promise.validationvalue);
                        // swal("Student has not mapped with any Package / Student has paid amount for that term")
                        if ($scope.terms_groups.length > 0) {
                            swal("Staff has paid amount for Selected " + terms_OR_groups);
                        }
                        else if ($scope.terms_groups.length == 0) {
                            $scope.Head_Instl_list = promise.mapped_hds_ins;
                        }

                        //swal("Student has paid amount for that Group");
                    }
                })
            //   }
        };

        $scope.tobepaidamt_st = function (Head_Instl_list, index) {
            var count = 0;

            //for (var r = 0; r < Head_Instl_list.length; r++)
            //{
            //    if(r==index)
            //    {

            //    }
            //}


            angular.forEach($scope.Head_Instl_list, function (user) {
                if (!!user.isSelected) {
                    count = count + 1
                    if (user.toBePaid == "" || user.toBePaid == undefined) {
                        user.toBePaid = 0;
                    }
                }
            })

            if (count <= 1) {
                $scope.FYP_Tot_Amount = Number(Head_Instl_list[index].toBePaid) + Number(Head_Instl_list[index].fineAmount);
            }
            else if (count > 1) {
                //if (Number(Head_Instl_list[index].toBePaid) + Number(Head_Instl_list[index].concessionAmount) <= Number(Head_Instl_list[index].totalamount)) {


                //    var intertobepaidamt = 0
                //    angular.forEach($scope.Head_Instl_list, function (user) {
                //        if (!!user.isSelected) {

                //            intertobepaidamt = Number(intertobepaidamt) + Number(user.toBePaid);
                //        }
                //    })
                //    $scope.curramount = intertobepaidamt
                //}

                //else {
                var intertobepaidamt = 0
                angular.forEach($scope.Head_Instl_list, function (user) {
                    if (!!user.isSelected) {
                        // intertobepaidamt = Number(intertobepaidamt) + Number(totalgrid[key].fsS_ToBePaid);

                        intertobepaidamt = Number(intertobepaidamt) + Number(user.toBePaid);
                    }
                })
                $scope.FYP_Tot_Amount = intertobepaidamt
                //    }

            }
        };
        $scope.amtdetails_st = function (userdata, Head_Instl_list, index) {
            trp = [];


            var newCol = "";
            newCol = {
                user_data: userdata, total_grid: Head_Instl_list, in_dex: index
            }
            trp.push(newCol);
            $scope.disablefine = true;
            $scope.disablenetamount = true;

            $scope.all = $scope.Head_Instl_list.every(function (itm) {
                return itm.isSelected;
            });

            if (Head_Instl_list[index].isSelected == true) {
                $scope.FYP_Tot_Concession_Amt = Number($scope.FYP_Tot_Concession_Amt) + Number(Head_Instl_list[index].concessionAmount);
                $scope.FYP_Tot_Fine_Amt = Number($scope.FYP_Tot_Fine_Amt) + Number(Head_Instl_list[index].fineAmount);
                $scope.FYP_Tot_Amount = Number($scope.FYP_Tot_Amount) + Number(Head_Instl_list[index].toBePaid) + Number(Head_Instl_list[index].fineAmount);
                //$scope.totalwaived = Number($scope.totalwaived) + Number(Head_Instl_list[index].fsS_WaivedAmount);
            }
            else if (Head_Instl_list[index].isSelected == false) {
                $scope.FYP_Tot_Concession_Amt = Number($scope.FYP_Tot_Concession_Amt) - Number(Head_Instl_list[index].concessionAmount);
                $scope.FYP_Tot_Fine_Amt = Number($scope.FYP_Tot_Fine_Amt) - Number(Head_Instl_list[index].fineAmount);
                $scope.FYP_Tot_Amount = Number($scope.FYP_Tot_Amount) - Number(Head_Instl_list[index].toBePaid) - Number(Head_Instl_list[index].fineAmount);
                // $scope.totalwaived = Number($scope.totalwaived) - Number(Head_Instl_list[index].fsS_WaivedAmount);
            }
            if (autoreceipt == 1) {
                $scope.get_grp_reptno(index);
            }

        };
        $scope.all_t_disable = false;
        $scope.all_g_disable = false;
        $scope.toggleAll_st = function (allchkdata) {


            $scope.disablefine = true;
            $scope.disablenetamount = true;
            $scope.disableconcession = true;


            var toggleStatus = $scope.all;

            $scope.FYP_Tot_Amount = 0;
            $scope.FYP_Tot_Concession_Amt = 0;
            $scope.FYP_Tot_Fine_Amt = 0;
            $scope.totalwaived = 0;

            angular.forEach($scope.Head_Instl_list, function (itm) {
                itm.isSelected = toggleStatus;
            });
            if (allchkdata == true) {

                for (var index = 0; index < $scope.Head_Instl_list.length; index++) {
                    $scope.FYP_Tot_Concession_Amt = Number($scope.FYP_Tot_Concession_Amt) + Number($scope.Head_Instl_list[index].concessionAmount);
                    $scope.FYP_Tot_Fine_Amt = Number($scope.FYP_Tot_Fine_Amt) + Number($scope.Head_Instl_list[index].fineAmount);
                    $scope.FYP_Tot_Amount = Number($scope.FYP_Tot_Amount) + Number($scope.Head_Instl_list[index].toBePaid) + Number($scope.Head_Instl_list[index].fineAmount);
                    //   $scope.totalwaived = Number($scope.totalwaived) + Number($scope.Head_Instl_list[index].fsS_WaivedAmount);
                }

            }
            else {
                $scope.FYP_Tot_Concession_Amt = 0;
                $scope.FYP_Tot_Fine_Amt = 0;
                $scope.FYP_Tot_Amount = 0;
                $scope.totalwaived = 0;
            }

            if (autoreceipt == 1) {
                $scope.get_grp_reptno();
            }

        }

        $scope.get_dates = function (yr_id, data) {
            var iddata = yr_id;


            if (data != null) {

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

        $scope.savedata_st = function (Head_Instl_list) {
            var count = 0, fypid = 0;
            var save_hes_insts = [];
            angular.forEach(Head_Instl_list, function (opq) {

                fypid = opq.fyP_Id;
                if (opq.isSelected) {
                    count += 1;
                    //  opq.netAmount = Number(opq.netAmount);
                    save_hes_insts.push(opq);
                }
            })
            if (count > 0) {
                if ($scope.FYP_Tot_Amount > 0) {
                    if ($scope.myForm.$valid) {
                        //if ($scope.FYP_Id > 0 ) {
                        //    var disfun = "Update";
                        //    var data = {
                        //        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        //        "Amst_Id": $scope.FMOST_Id,
                        //        savetmpdata: $scope.savedatatrans,
                        //        "FYP_Receipt_No": $scope.FYP_Receipt_No,
                        //        "FYP_Date": new Date($scope.FYP_Date),
                        //        "FYP_Remarks": $scope.FYP_Remarks,
                        //        "FYP_Bank_Or_Cash": $scope.FYP_Bank_Or_Cash,
                        //        // "L_Code": $scope.L_Code,
                        //        "FYP_DD_Cheque_Date": new Date($scope.FYP_DD_Cheque_Date).toDateString(),
                        //        "FYP_DD_Cheque_No": $scope.FYP_DD_Cheque_No,
                        //        "FYP_Tot_Amount": $scope.curramount,
                        //        "FYP_Tot_Concession_Amt": $scope.totalconcession,
                        //        "FYP_Tot_Fine_Amt": $scope.totalfine,
                        //        "FYP_Tot_Waived_Amt": $scope.totalwaived,
                        //        "FYP_Bank_Name": $scope.FYP_Bank_Name,
                        //        "filterinitialdata": $scope.filterdata,
                        //        "auto_receipt_flag": autoreceipt,
                        //        "automanualreceiptno": automanualreceiptnotranum,
                        //        transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                        //        "FYP_Id": $scope.FYP_Id
                        //    }
                        //}
                        //  else {

                        //  var disfun = "Save";

                        if ($scope.FYP_Remarks == undefined || $scope.FYP_Remarks == null) {
                            $scope.FYP_Remarks = "";
                        }
                        if ($scope.FYP_Bank_Or_Cash == 'C') {
                            $scope.FYP_Bank_Name = "";
                            $scope.FYP_DD_Cheque_No = "";
                        }
                        if ($scope.filterdata == 'Staff') {
                            var data = {
                                // "FYP_Id": $scope.FYP_Id,

                                "FYP_Id": fypid,
                                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                                "FYP_Receipt_No": $scope.FYP_Receipt_No,
                                "FYP_Bank_Name": $scope.FYP_Bank_Name,
                                "FYP_Bank_Or_Cash": $scope.FYP_Bank_Or_Cash,
                                "FYP_DD_Cheque_No": $scope.FYP_DD_Cheque_No,
                                "FYP_DD_Cheque_Date": new Date($scope.FYP_DD_Cheque_Date).toDateString(),
                                "FYP_Date": new Date($scope.FYP_Date).toDateString(),
                                "FYP_Tot_Amount": $scope.FYP_Tot_Amount,
                                // "FYP_Tot_Waived_Amt": $scope.totalwaived,
                                "FYP_Tot_Fine_Amt": $scope.FYP_Tot_Fine_Amt,
                                "FYP_Tot_Concession_Amt": $scope.FYP_Tot_Concession_Amt,
                                "FYP_Remarks": $scope.FYP_Remarks,
                                // "Amst_Id": $scope.FMOST_Id.fmosT_Id,
                                head_installments: save_hes_insts,
                                // "Grp_Term_flg": grouporterm,
                                "Stf_Others_flg": $scope.filterdata,
                                // "FMOST_Id": $scope.FMOST_Id,
                                "HRME_Id": $scope.HRME_Id.hrmE_Id,
                                "auto_receipt_flag": autoreceipt,
                                "automanualreceiptno": automanualreceiptnotranum,

                            }
                        }
                        else if ($scope.filterdata == 'Others') {
                            var data = {
                                "FYP_Id": fypid,
                                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                                "FYP_Receipt_No": $scope.FYP_Receipt_No,
                                "FYP_Bank_Name": $scope.FYP_Bank_Name,
                                "FYP_Bank_Or_Cash": $scope.FYP_Bank_Or_Cash,
                                "FYP_DD_Cheque_No": $scope.FYP_DD_Cheque_No,
                                "FYP_DD_Cheque_Date": new Date($scope.FYP_DD_Cheque_Date).toDateString(),
                                "FYP_Date": new Date($scope.FYP_Date).toDateString(),
                                "FYP_Tot_Amount": $scope.FYP_Tot_Amount,
                                // "FYP_Tot_Waived_Amt": $scope.totalwaived,
                                "FYP_Tot_Fine_Amt": $scope.FYP_Tot_Fine_Amt,
                                "FYP_Tot_Concession_Amt": $scope.FYP_Tot_Concession_Amt,
                                "FYP_Remarks": $scope.FYP_Remarks,
                                // "Amst_Id": $scope.FMOST_Id.fmosT_Id,
                                head_installments: save_hes_insts,
                                // "Grp_Term_flg": grouporterm,
                                "Stf_Others_flg": $scope.filterdata,
                                "FMOST_Id": $scope.FMOST_Id,
                                "auto_receipt_flag": autoreceipt,
                                "automanualreceiptno": automanualreceiptnotranum,
                                // "HRME_Id": $scope.HRME_Id,

                            }
                        }
                        // }
                        var config = {
                            headers: {
                                'Content-Type': 'application/json;'
                            }
                        }
                        //receipt

                        //swal({
                        //    title: "Are you sure?",
                        //    text: "Do You Want To " + disfun + " Record? ",
                        //    type: "warning",
                        //    showCancelButton: true,
                        //    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes," + disfun + " it",
                        //    cancelButtonText: "Cancel",
                        //    closeOnConfirm: false,
                        //    closeOnCancel: false,
                        //    showLoaderOnConfirm: true,

                        //},
                        //function (isConfirm) {
                        //    if (isConfirm) {


                        apiService.create("CollegeStaffAndOtherTransaction/savedata_st", data).
                            then(function (promise) {

                                if (promise.returnval == "Cancel") {
                                    swal("Transaction Failed");
                                }

                                if (promise.returnval == "Update") {
                                    swal("Transaction Updated Successfully");
                                }
                                //else if (promise.returnval == "Duplicate") {
                                //    swal('Group is already saved for the student');
                                //}
                                else if (promise.returnval == "Save") {
                                    swal('Transaction Done Successfully For Selected ' + $scope.filterdata);
                                }
                                else if (promise.returnval == "Error") {
                                    swal('Kindly contact Administrator ');
                                }

                                $state.reload();


                                //if (promise.returnval == "true") {
                                //    //reload

                                //    if ($scope.cfg.ASMAY_Id === promise.fillyear[0].asmaY_Id) {
                                //    }
                                //    else {
                                //        $scope.yearlst = promise.fillyear;
                                //        $scope.cfg.ASMAY_Id = promise.fillyear[0].asmaY_Id;
                                //    }

                                //    if (autoreceipt == 1) {
                                //        $scope.showreceiptno = false;
                                //    }
                                //    else {
                                //        $scope.showreceiptno = true;
                                //    }

                                //    $scope.receiptgrid = promise.receiparraydelete;

                                //    $scope.FYP_Date = new Date();
                                //    $scope.FYP_DD_Cheque_Date = new Date();

                                //    if ($scope.FYP_Bank_Or_Cash == 'C') {
                                //        $scope.bankdetails = false;
                                //    }
                                //    else if ($scope.FYP_Bank_Or_Cash == 'B') {
                                //        $scope.bankdetails = true;
                                //    }

                                //    //configuration settings
                                //    if (promise.feeconfiglist.length > 0) {
                                //        grouporterm = promise.feeconfiglist[0].fmC_GroupOrTermFlg;

                                //        if (grouporterm == "T") {
                                //            $scope.grouportername = "Term Name"
                                //        }
                                //        else if (grouporterm == "G") {
                                //            $scope.grouportername = "Group Name"
                                //        }
                                //    }

                                //    $scope.addnewbtn = true;

                                //    $scope.amst_Id = "";
                                //    $scope.FYP_Receipt_No = "";
                                //    $scope.groupcount = "";
                                //    $scope.amtadjustment = "";
                                //    $scope.FYP_Remarks = "";
                                //    $scope.FYP_Bank_Or_Cash = "";
                                //    $scope.L_Code = "";

                                //    $scope.FYP_DD_Cheque_No = "";
                                //    $scope.FYP_Bank_Name = "";
                                //    $scope.totalfee = "";
                                //    $scope.totalconcession = "";
                                //    $scope.totalfine = "";
                                //    $scope.curramount = "";
                                //    $scope.currbalance = "";
                                //    $scope.totalwaived = "";
                                //    $scope.grigview1 = false;

                                //    $scope.submitted = false;

                                //    $scope.FYP_Date = new Date();
                                //    $scope.FYP_DD_Cheque_Date = new Date();

                                //    $state.reload();
                                //    swal("Record " + promise.displaymessage + " Successfully")
                                //}

                                //else if (promise.returnval == "false") {
                                //    swal("Record " + promise.displaymessage + " Successfully")
                                //    //swal("Kindly contact Administrator");
                                //}
                                //else {
                                //    swal(promise.returnval);
                                //}

                                //$state.reload();

                                // $("#myModal1").modal();


                            })
                        // }
                        //else {
                        //    swal("Record saved Failed", "Failed");
                        //}


                        //});


                    }
                    else {
                        $scope.submitted = true;
                    }
                }
                else {
                    swal("We Can't Make Transaction For Amount Zero!!!");
                }

            }
            else {
                swal("Atleast One Head Has To Be Checked To Save The Transaction!!!");
            }


        };

        $scope.showmodaldetails_s = function (fypid, stafid) {

            $scope.ASMCL_ClassName = "";
            $scope.ASMC_SectionName = "";
            $scope.AMAY_RollNo = "";
            $scope.AMST_AdmNo = "";
            $scope.AMST_FirstName = "";
            //$scope.curdate = new Date().getDate() + '/' + new Date().getMonth() + '/' + new Date().getFullYear();
            $scope.curdate = "";
            $scope.asmaY_Year = "";
            $scope.AMST_FatherName = "";
            $scope.AMST_MotherName = "";
            $scope.receiptno = "";
            $scope.FMCC_ConcessionName = "";

            var data = {
                "FYP_Id": fypid,
                "HRME_Id": stafid,
                "ASMAY_Id": $scope.cfg.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("CollegeStaffAndOtherTransaction/printreceipt_s", data).
                then(function (promise) {
                    $scope.colspan = 3;
                    // mergeinstallment = 1;
                    $scope.Merge_Installments = mergeinstallment;
                    if (mergeinstallment == 1) {
                        var installments_merged_list = [];
                        angular.forEach(promise.receiptdetails, function (wed) {
                            var head_id = wed.fmH_Id;
                            var toBePaid = 0, ftposT_ConcessionAmount = 0, ftposT_PaidAmount = 0;
                            //fyP_Tot_Amount = 0,
                            angular.forEach(promise.receiptdetails, function (wed1) {
                                if (wed1.fmH_Id == head_id) {
                                    toBePaid += wed1.toBePaid;
                                    ftposT_PaidAmount += wed1.ftposT_PaidAmount;
                                    ftposT_ConcessionAmount += wed1.ftposT_ConcessionAmount;
                                    // fyP_Tot_Amount += wed1.fyP_Tot_Amount;
                                }

                            })
                            wed.toBePaid = toBePaid;
                            wed.ftposT_ConcessionAmount = ftposT_ConcessionAmount;
                            wed.ftposT_PaidAmount = ftposT_PaidAmount;
                            if (installments_merged_list.length == 0) {
                                installments_merged_list.push(wed);
                            }
                            else if (installments_merged_list.length > 0) {
                                var head_alrdy_cnt = 0;
                                angular.forEach(installments_merged_list, function (wed2) {
                                    if (wed2.fmH_Id == head_id) {
                                        head_alrdy_cnt += 1;
                                    }
                                })
                                if (head_alrdy_cnt == 0) {
                                    installments_merged_list.push(wed);
                                }
                            }
                        })
                        promise.receiptdetails = installments_merged_list;
                        $scope.colspan = 2;
                    }
                    $scope.staff_fee_receipt_details = promise.receiptdetails;

                    var tot_toBePaid = 0, tot_ftposT_ConcessionAmount = 0, tot_ftposT_PaidAmount = 0;
                    angular.forEach(promise.receiptdetails, function (wed1) {

                        tot_toBePaid += wed1.toBePaid;
                        tot_ftposT_ConcessionAmount += wed1.ftposT_ConcessionAmount;
                        tot_ftposT_PaidAmount += wed1.ftposT_PaidAmount;

                    })
                    $scope.tot_toBePaid = tot_toBePaid;
                    $scope.tot_ftposT_ConcessionAmount = tot_ftposT_ConcessionAmount;
                    $scope.tot_ftposT_PaidAmount = tot_ftposT_PaidAmount;
                    $scope.tot_in_words = $scope.amountinwords($scope.tot_ftposT_PaidAmount);
                    $scope.Next_due_amount = promise.dueamount;

                    $scope.period = promise.duration;
                    $scope.Next_due_date = promise.due_Date;

                    $scope.paymenrgrid = promise.currpaymentdetails;
                    $scope.atotA = promise.currpaymentdetails[0].ftP_Paid_Amt;
                    $scope.ctotA = promise.currpaymentdetails[0].ftP_Concession_Amt;
                    $scope.totchar = $scope.atotA + $scope.ctotA;

                    $scope.words = $scope.amountinwords($scope.atotA);

                    $scope.year = promise.year;


                    if ($scope.due_amount == 0) {
                        $scope.date = "";
                        $scope.nextduedate = false;
                    }
                    else {
                        $scope.date = promise.date;
                        $scope.nextduedate = true;
                    }

                    if ($scope.due_amount == 0) {
                        $scope.months = "";
                    }
                    else {
                        $scope.months = promise.month;

                        $scope.nextduedate = true;
                    }

                    var termname = " ";
                    if (promise.termremarks.length > 0) {
                        for (var i = 0; i < promise.termremarks.length; i++) {
                            if (termname == " ") {
                                termname = promise.termremarks[i].termname
                            }
                            else {
                                termname = termname + ',' + promise.termremarks[i].termname
                            }
                        }
                    }

                    var feeheadname = "";
                    var validation;
                    $scope.tempreceiptarraytermexfinal = [];
                    if (receiptformat == 1 || receiptformat == 0) {
                        $scope.tempreceiptarray = [];
                        $scope.tempreceiptarrayterm = {
                        };
                        $scope.tempreceiptarraytermex = {
                        };
                        var totalamount = 0, concessionamt = 0, fineamt = 0, feecount = 0, fmH_FeeName, feetotcharges = 0;
                        var totalamountex = 0, concessionamtex = 0, fineamtex = 0, fmH_FeeNameex, feetotchargesex = 0;
                        if (promise.fillstudentviewdetails.length > 0) {
                            for (var i = 0; i < promise.fillstudentviewdetails.length; i++) {
                                var mainheadid = promise.fillstudentviewdetails[i].fmH_Id;
                                var maininstallmentid = promise.fillstudentviewdetails[i].ftI_Id;
                                if (promise.receiptformathead.length > 0) {
                                    for (var j = 0; j < promise.receiptformathead.length; j++) {
                                        var subheadid = promise.receiptformathead[j].fmH_Id;
                                        var subinstid = promise.receiptformathead[j].ftI_Id;

                                        if (mainheadid == subheadid && maininstallmentid == subinstid) {
                                            feecount = Number(feecount) + 1;

                                            feetotcharges = Number(feetotcharges) + Number(promise.receiptformathead[j].totalcharges);

                                            fmH_FeeName = promise.receiptformathead[j].fmH_FeeName
                                            totalamount = Number(totalamount) + Number(promise.receiptformathead[j].ftP_Paid_Amt);
                                            concessionamt = Number(concessionamt) + Number(promise.receiptformathead[j].ftP_Concession_Amt);
                                            fineamt = Number(fineamt) + Number(promise.receiptformathead[j].ftP_Fine_Amt);
                                        }
                                    }
                                    if (feecount < 1) {

                                        fmH_FeeNameex = promise.fillstudentviewdetails[i].fmH_FeeName
                                        if (feeheadname == "") {
                                            feetotchargesex = Number(promise.fillstudentviewdetails[i].totalcharges);

                                            totalamountex = Number(promise.fillstudentviewdetails[i].ftP_Paid_Amt);
                                            concessionamtex = Number(promise.fillstudentviewdetails[i].ftP_Concession_Amt);
                                            fineamtex = Number(promise.fillstudentviewdetails[i].ftP_Fine_Amt);
                                        }

                                        else if (fmH_FeeNameex === feeheadname) {
                                            feetotchargesex = Number(feetotchargesex) + Number(promise.fillstudentviewdetails[i].totalcharges);

                                            totalamountex = Number(totalamountex) + Number(promise.fillstudentviewdetails[i].ftP_Paid_Amt);
                                            concessionamtex = Number(concessionamtex) + Number(promise.fillstudentviewdetails[i].ftP_Concession_Amt);
                                            fineamtex = Number(fineamtex) + Number(promise.fillstudentviewdetails[i].ftP_Fine_Amt);
                                        }
                                        else {
                                            validation = "add";

                                            $scope.tempreceiptarraytermex = {
                                                fmH_FeeName: feeheadname,
                                                ftP_Paid_Amt: totalamountex,
                                                ftP_Concession_Amt: concessionamtex,
                                                ftp_fine_amt: fineamtex,
                                                totalcharges: feetotchargesex,
                                            };

                                            $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);

                                            fmH_FeeNameex = promise.fillstudentviewdetails[i].fmH_FeeName
                                            totalamountex = Number(promise.fillstudentviewdetails[i].ftP_Paid_Amt);
                                            concessionamtex = Number(promise.fillstudentviewdetails[i].ftP_Concession_Amt);
                                            fineamtex = Number(promise.fillstudentviewdetails[i].ftP_Fine_Amt);

                                            feetotchargesex = Number(promise.fillstudentviewdetails[i].totalcharges);

                                            $scope.tempreceiptarraytermex = {
                                                fmH_FeeName: fmH_FeeNameex,
                                                ftP_Paid_Amt: totalamountex,
                                                ftP_Concession_Amt: concessionamtex,
                                                ftp_fine_amt: fineamtex,
                                                totalcharges: feetotchargesex,
                                            };

                                            $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);
                                        }

                                        feeheadname = fmH_FeeNameex

                                    }
                                    feecount = 0;
                                }
                                else {
                                    $scope.tempreceiptarray.push(promise.fillstudentviewdetails[i]);
                                }
                            }

                        }

                        if (promise.receiptformathead.length > 0) {
                            var temlength = $scope.tempreceiptarray.length;

                            $scope.tempreceiptarrayterm = {
                                fmH_FeeName: fmH_FeeName,
                                ftP_Paid_Amt: totalamount,
                                ftP_Concession_Amt: concessionamt,
                                ftp_fine_amt: fineamt,
                                totalcharges: feetotcharges,
                            };

                            if (validation != "add") {
                                $scope.tempreceiptarraytermex = {
                                    fmH_FeeName: fmH_FeeNameex,
                                    ftP_Paid_Amt: totalamountex,
                                    ftP_Concession_Amt: concessionamtex,
                                    ftp_fine_amt: fineamtex,
                                    totalcharges: feetotchargesex,
                                };

                                $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);
                            }

                            $scope.tempreceiptarray.push($scope.tempreceiptarrayterm);

                            for (var r = 0; r < $scope.tempreceiptarraytermexfinal.length; r++) {
                                if ($scope.tempreceiptarraytermexfinal[r].fmH_FeeName != undefined) {
                                    $scope.tempreceiptarray.push($scope.tempreceiptarraytermexfinal[r]);
                                }
                            }
                        }

                        $scope.showdetailsreceipt = $scope.tempreceiptarray;
                        $scope.showtotaldetails = promise.filltotaldetails;

                    }
                    else {
                        $scope.showdetailsreceipt = promise.fillstudentviewdetails;
                        $scope.showtotaldetails = promise.filltotaldetails;

                    }

                    if (promise.fillstudentviewdetails[0].amsT_FirstName != null && promise.fillstudentviewdetails[0].amsT_MiddleName != null && promise.fillstudentviewdetails[0].amsT_LastName != null) {
                        $scope.AMST_FirstName = promise.fillstudentviewdetails[0].amsT_FirstName + ' ' + promise.fillstudentviewdetails[0].amsT_MiddleName + ' ' + promise.fillstudentviewdetails[0].amsT_LastName;
                    }
                    else {
                        if (promise.fillstudentviewdetails[0].amsT_FirstName == null) {
                            promise.fillstudentviewdetails[0].amsT_FirstName = ' ';
                        }
                        if (promise.fillstudentviewdetails[0].amsT_MiddleName == null) {
                            promise.fillstudentviewdetails[0].amsT_MiddleName = ' ';
                        }
                        if (promise.fillstudentviewdetails[0].amsT_LastName == null) {
                            promise.fillstudentviewdetails[0].amsT_LastName = ' ';
                        }
                        $scope.AMST_FirstName = promise.fillstudentviewdetails[0].amsT_FirstName + ' ' + promise.fillstudentviewdetails[0].amsT_MiddleName + ' ' + promise.fillstudentviewdetails[0].amsT_LastName;
                    }


                    $scope.ASMCL_ClassName = promise.fillstudentviewdetails[0].classname;
                    $scope.ASMC_SectionName = promise.fillstudentviewdetails[0].sectionname;
                    $scope.AMAY_RollNo = promise.fillstudentviewdetails[0].rollno;
                    $scope.AMST_AdmNo = promise.fillstudentviewdetails[0].admno;
                    $scope.curdate = $filter('date')(new Date(), "dd-MM-yyyy");
                    $scope.asmaY_Year = promise.asmaY_Year;
                    $scope.AMST_FatherName = promise.fillstudentviewdetails[0].fathername;
                    $scope.AMST_MotherName = promise.fillstudentviewdetails[0].mothername;
                    $scope.receiptno = promise.fillstudentviewdetails[0].fyP_Receipt_No;

                    $scope.FMCC_ConcessionName = promise.fillstudentviewdetails[0].fmcC_ConcessionName;

                    $scope.FYP_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_Date, "dd-MM-yyyy");

                    if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "C") {
                        $scope.bankdet = false;
                        $scope.FYP_Bank_Or_Cash = "Cash";
                    }

                    if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "O") {
                        $scope.bankdet = false;
                        $scope.FYP_Bank_Or_Cash = "Online Payment";
                    }

                    if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "B") {
                        $scope.bankdet = true;
                        $scope.FYP_Bank_Or_Cash = "Bank";
                        $scope.FYP_DD_Cheque_No = promise.fillstudentviewdetails[0].fyP_DD_Cheque_No;
                        $scope.FYP_DD_Cheque_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_DD_Cheque_Date, "dd-MM-yyyy");
                        $scope.FYP_Bank_Name = promise.fillstudentviewdetails[0].fyP_Bank_Name;
                    }

                    if (promise.fillstudentviewdetails[0].fyP_Remarks != null || promise.fillstudentviewdetails[0].fyP_Remarks != "") {
                        //$scope.feeremarks = promise.fillstudentviewdetails[0].fyP_Remarks; 
                        $scope.feeremarks = termname;
                    }
                    else {
                        $scope.feeremarks = "Remarks Not Given";
                    }

                })

            $scope.atotalA = function (e) {
                var atotalc = 0;
                angular.forEach($scope.showdetailsreceipt, function (e) {
                    atotalc += e.ftP_Paid_Amt;
                });
                return atotalc;
            };
            $scope.ctotalA = function (e) {
                var atotalc = 0;
                angular.forEach($scope.showdetailsreceipt, function (e) {
                    atotalc += e.ftP_Concession_Amt;
                });
                return atotalc;
            };

        }

        $scope.showmodaldetails_o = function (fypid, stuid) {

            $scope.ASMCL_ClassName = "";
            $scope.ASMC_SectionName = "";
            $scope.AMAY_RollNo = "";
            $scope.AMST_AdmNo = "";
            $scope.AMST_FirstName = "";
            //$scope.curdate = new Date().getDate() + '/' + new Date().getMonth() + '/' + new Date().getFullYear();
            $scope.curdate = "";
            $scope.asmaY_Year = "";
            $scope.AMST_FatherName = "";
            $scope.AMST_MotherName = "";
            $scope.receiptno = "";
            $scope.FMCC_ConcessionName = "";

            var data = {
                "FYP_Id": fypid,
                "FMOST_Id": stuid,
                "ASMAY_Id": $scope.cfg.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("CollegeStaffAndOtherTransaction/printreceipt_o", data).
                then(function (promise) {
                    $scope.colspan = 3;
                    // mergeinstallment = 1;
                    $scope.Merge_Installments = mergeinstallment;
                    if (mergeinstallment == 1) {
                        var installments_merged_list = [];
                        angular.forEach(promise.receiptdetails, function (wed) {
                            var head_id = wed.fmH_Id;
                            var toBePaid = 0, ftposT_ConcessionAmount = 0, ftposT_PaidAmount = 0;
                            //fyP_Tot_Amount = 0,
                            angular.forEach(promise.receiptdetails, function (wed1) {
                                if (wed1.fmH_Id == head_id) {
                                    toBePaid += wed1.toBePaid;
                                    ftposT_PaidAmount += wed1.ftposT_PaidAmount;
                                    ftposT_ConcessionAmount += wed1.ftposT_ConcessionAmount;
                                    // fyP_Tot_Amount += wed1.fyP_Tot_Amount;
                                }

                            })
                            wed.toBePaid = toBePaid;
                            wed.ftposT_ConcessionAmount = ftposT_ConcessionAmount;
                            wed.ftposT_PaidAmount = ftposT_PaidAmount;
                            if (installments_merged_list.length == 0) {
                                installments_merged_list.push(wed);
                            }
                            else if (installments_merged_list.length > 0) {
                                var head_alrdy_cnt = 0;
                                angular.forEach(installments_merged_list, function (wed2) {
                                    if (wed2.fmH_Id == head_id) {
                                        head_alrdy_cnt += 1;
                                    }
                                })
                                if (head_alrdy_cnt == 0) {
                                    installments_merged_list.push(wed);
                                }
                            }
                        })
                        promise.receiptdetails = installments_merged_list;
                        $scope.colspan = 2;
                    }
                    $scope.others_fee_receipt_details = promise.receiptdetails;

                    var tot_toBePaid = 0, tot_ftposT_ConcessionAmount = 0, tot_ftposT_PaidAmount = 0;
                    angular.forEach(promise.receiptdetails, function (wed1) {

                        tot_toBePaid += wed1.toBePaid;
                        tot_ftposT_ConcessionAmount += wed1.ftposT_ConcessionAmount;
                        tot_ftposT_PaidAmount += wed1.ftposT_PaidAmount;

                    })
                    $scope.tot_toBePaid = tot_toBePaid;
                    $scope.tot_ftposT_ConcessionAmount = tot_ftposT_ConcessionAmount;
                    $scope.tot_ftposT_PaidAmount = tot_ftposT_PaidAmount;

                    $scope.tot_in_words = $scope.amountinwords($scope.tot_ftposT_PaidAmount);
                    $scope.Next_due_amount = promise.dueamount;

                    $scope.period = promise.duration;
                    $scope.Next_due_date = promise.due_Date;

                    $scope.paymenrgrid = promise.currpaymentdetails;
                    $scope.atotA = promise.currpaymentdetails[0].ftP_Paid_Amt;
                    $scope.ctotA = promise.currpaymentdetails[0].ftP_Concession_Amt;
                    $scope.totchar = $scope.atotA + $scope.ctotA;

                    $scope.words = $scope.amountinwords($scope.atotA);

                    $scope.year = promise.year;

                    $scope.due_amount = promise.dueamount;
                    if ($scope.due_amount == 0) {
                        $scope.date = "";
                        $scope.nextduedate = false;
                    }
                    else {
                        $scope.date = promise.date;
                        $scope.nextduedate = true;
                    }

                    if ($scope.due_amount == 0) {
                        $scope.months = "";
                    }
                    else {
                        $scope.months = promise.month;

                        $scope.nextduedate = true;
                    }

                    var termname = " ";
                    if (promise.termremarks.length > 0) {
                        for (var i = 0; i < promise.termremarks.length; i++) {
                            if (termname == " ") {
                                termname = promise.termremarks[i].termname
                            }
                            else {
                                termname = termname + ',' + promise.termremarks[i].termname
                            }
                        }
                    }

                    var feeheadname = "";
                    var validation;
                    $scope.tempreceiptarraytermexfinal = [];
                    if (receiptformat == 1 || receiptformat == 0) {
                        $scope.tempreceiptarray = [];
                        $scope.tempreceiptarrayterm = {
                        };
                        $scope.tempreceiptarraytermex = {
                        };
                        var totalamount = 0, concessionamt = 0, fineamt = 0, feecount = 0, fmH_FeeName, feetotcharges = 0;
                        var totalamountex = 0, concessionamtex = 0, fineamtex = 0, fmH_FeeNameex, feetotchargesex = 0;
                        if (promise.fillstudentviewdetails.length > 0) {
                            for (var i = 0; i < promise.fillstudentviewdetails.length; i++) {
                                var mainheadid = promise.fillstudentviewdetails[i].fmH_Id;
                                var maininstallmentid = promise.fillstudentviewdetails[i].ftI_Id;
                                if (promise.receiptformathead.length > 0) {
                                    for (var j = 0; j < promise.receiptformathead.length; j++) {
                                        var subheadid = promise.receiptformathead[j].fmH_Id;
                                        var subinstid = promise.receiptformathead[j].ftI_Id;

                                        if (mainheadid == subheadid && maininstallmentid == subinstid) {
                                            feecount = Number(feecount) + 1;

                                            feetotcharges = Number(feetotcharges) + Number(promise.receiptformathead[j].totalcharges);

                                            fmH_FeeName = promise.receiptformathead[j].fmH_FeeName
                                            totalamount = Number(totalamount) + Number(promise.receiptformathead[j].ftP_Paid_Amt);
                                            concessionamt = Number(concessionamt) + Number(promise.receiptformathead[j].ftP_Concession_Amt);
                                            fineamt = Number(fineamt) + Number(promise.receiptformathead[j].ftP_Fine_Amt);
                                        }
                                    }
                                    if (feecount < 1) {

                                        fmH_FeeNameex = promise.fillstudentviewdetails[i].fmH_FeeName
                                        if (feeheadname == "") {
                                            feetotchargesex = Number(promise.fillstudentviewdetails[i].totalcharges);

                                            totalamountex = Number(promise.fillstudentviewdetails[i].ftP_Paid_Amt);
                                            concessionamtex = Number(promise.fillstudentviewdetails[i].ftP_Concession_Amt);
                                            fineamtex = Number(promise.fillstudentviewdetails[i].ftP_Fine_Amt);
                                        }

                                        else if (fmH_FeeNameex === feeheadname) {
                                            feetotchargesex = Number(feetotchargesex) + Number(promise.fillstudentviewdetails[i].totalcharges);

                                            totalamountex = Number(totalamountex) + Number(promise.fillstudentviewdetails[i].ftP_Paid_Amt);
                                            concessionamtex = Number(concessionamtex) + Number(promise.fillstudentviewdetails[i].ftP_Concession_Amt);
                                            fineamtex = Number(fineamtex) + Number(promise.fillstudentviewdetails[i].ftP_Fine_Amt);
                                        }
                                        else {
                                            validation = "add";

                                            $scope.tempreceiptarraytermex = {
                                                fmH_FeeName: feeheadname,
                                                ftP_Paid_Amt: totalamountex,
                                                ftP_Concession_Amt: concessionamtex,
                                                ftp_fine_amt: fineamtex,
                                                totalcharges: feetotchargesex,
                                            };

                                            $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);

                                            fmH_FeeNameex = promise.fillstudentviewdetails[i].fmH_FeeName
                                            totalamountex = Number(promise.fillstudentviewdetails[i].ftP_Paid_Amt);
                                            concessionamtex = Number(promise.fillstudentviewdetails[i].ftP_Concession_Amt);
                                            fineamtex = Number(promise.fillstudentviewdetails[i].ftP_Fine_Amt);

                                            feetotchargesex = Number(promise.fillstudentviewdetails[i].totalcharges);

                                            $scope.tempreceiptarraytermex = {
                                                fmH_FeeName: fmH_FeeNameex,
                                                ftP_Paid_Amt: totalamountex,
                                                ftP_Concession_Amt: concessionamtex,
                                                ftp_fine_amt: fineamtex,
                                                totalcharges: feetotchargesex,
                                            };

                                            $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);
                                        }

                                        feeheadname = fmH_FeeNameex

                                    }
                                    feecount = 0;
                                }
                                else {
                                    $scope.tempreceiptarray.push(promise.fillstudentviewdetails[i]);
                                }
                            }

                        }

                        if (promise.receiptformathead.length > 0) {
                            var temlength = $scope.tempreceiptarray.length;

                            $scope.tempreceiptarrayterm = {
                                fmH_FeeName: fmH_FeeName,
                                ftP_Paid_Amt: totalamount,
                                ftP_Concession_Amt: concessionamt,
                                ftp_fine_amt: fineamt,
                                totalcharges: feetotcharges,
                            };

                            if (validation != "add") {
                                $scope.tempreceiptarraytermex = {
                                    fmH_FeeName: fmH_FeeNameex,
                                    ftP_Paid_Amt: totalamountex,
                                    ftP_Concession_Amt: concessionamtex,
                                    ftp_fine_amt: fineamtex,
                                    totalcharges: feetotchargesex,
                                };

                                $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);
                            }

                            $scope.tempreceiptarray.push($scope.tempreceiptarrayterm);

                            for (var r = 0; r < $scope.tempreceiptarraytermexfinal.length; r++) {
                                if ($scope.tempreceiptarraytermexfinal[r].fmH_FeeName != undefined) {
                                    $scope.tempreceiptarray.push($scope.tempreceiptarraytermexfinal[r]);
                                }
                            }
                        }

                        $scope.showdetailsreceipt = $scope.tempreceiptarray;
                        $scope.showtotaldetails = promise.filltotaldetails;

                    }
                    else {
                        $scope.showdetailsreceipt = promise.fillstudentviewdetails;
                        $scope.showtotaldetails = promise.filltotaldetails;

                    }

                    if (promise.fillstudentviewdetails[0].amsT_FirstName != null && promise.fillstudentviewdetails[0].amsT_MiddleName != null && promise.fillstudentviewdetails[0].amsT_LastName != null) {
                        $scope.AMST_FirstName = promise.fillstudentviewdetails[0].amsT_FirstName + ' ' + promise.fillstudentviewdetails[0].amsT_MiddleName + ' ' + promise.fillstudentviewdetails[0].amsT_LastName;
                    }
                    else {
                        if (promise.fillstudentviewdetails[0].amsT_FirstName == null) {
                            promise.fillstudentviewdetails[0].amsT_FirstName = ' ';
                        }
                        if (promise.fillstudentviewdetails[0].amsT_MiddleName == null) {
                            promise.fillstudentviewdetails[0].amsT_MiddleName = ' ';
                        }
                        if (promise.fillstudentviewdetails[0].amsT_LastName == null) {
                            promise.fillstudentviewdetails[0].amsT_LastName = ' ';
                        }
                        $scope.AMST_FirstName = promise.fillstudentviewdetails[0].amsT_FirstName + ' ' + promise.fillstudentviewdetails[0].amsT_MiddleName + ' ' + promise.fillstudentviewdetails[0].amsT_LastName;
                    }


                    $scope.ASMCL_ClassName = promise.fillstudentviewdetails[0].classname;
                    $scope.ASMC_SectionName = promise.fillstudentviewdetails[0].sectionname;
                    $scope.AMAY_RollNo = promise.fillstudentviewdetails[0].rollno;
                    $scope.AMST_AdmNo = promise.fillstudentviewdetails[0].admno;
                    $scope.curdate = $filter('date')(new Date(), "dd-MM-yyyy");
                    $scope.asmaY_Year = promise.asmaY_Year;
                    $scope.AMST_FatherName = promise.fillstudentviewdetails[0].fathername;
                    $scope.AMST_MotherName = promise.fillstudentviewdetails[0].mothername;
                    $scope.receiptno = promise.fillstudentviewdetails[0].fyP_Receipt_No;

                    $scope.FMCC_ConcessionName = promise.fillstudentviewdetails[0].fmcC_ConcessionName;

                    $scope.FYP_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_Date, "dd-MM-yyyy");

                    if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "C") {
                        $scope.bankdet = false;
                        $scope.FYP_Bank_Or_Cash = "Cash";
                    }

                    if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "O") {
                        $scope.bankdet = false;
                        $scope.FYP_Bank_Or_Cash = "Online Payment";
                    }

                    if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "B") {
                        $scope.bankdet = true;
                        $scope.FYP_Bank_Or_Cash = "Bank";
                        $scope.FYP_DD_Cheque_No = promise.fillstudentviewdetails[0].fyP_DD_Cheque_No;
                        $scope.FYP_DD_Cheque_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_DD_Cheque_Date, "dd-MM-yyyy");
                        $scope.FYP_Bank_Name = promise.fillstudentviewdetails[0].fyP_Bank_Name;
                    }

                    if (promise.fillstudentviewdetails[0].fyP_Remarks != null || promise.fillstudentviewdetails[0].fyP_Remarks != "") {
                        //$scope.feeremarks = promise.fillstudentviewdetails[0].fyP_Remarks; 
                        $scope.feeremarks = termname;
                    }
                    else {
                        $scope.feeremarks = "Remarks Not Given";
                    }

                })

            $scope.atotalA = function (e) {
                var atotalc = 0;
                angular.forEach($scope.showdetailsreceipt, function (e) {
                    atotalc += e.ftP_Paid_Amt;
                });
                return atotalc;
            };
            $scope.ctotalA = function (e) {
                var atotalc = 0;
                angular.forEach($scope.showdetailsreceipt, function (e) {
                    atotalc += e.ftP_Concession_Amt;
                });
                return atotalc;
            };

        }
        $scope.printData_s = function () {
            var innerContents = document.getElementById("printmodal_s").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link href="plugins/bootstrap/css/bootstrap.css" />' +
                '<link href="css/style.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.printData_o = function () {
            var innerContents = document.getElementById("printmodal_o").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link href="plugins/bootstrap/css/bootstrap.css" />' +
                '<link href="css/style.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.DeletRecord_s = function (paymentid, stfid) {


            var data = {
                "FYP_Id": paymentid,
                "HRME_Id": stfid,
                "ASMAY_Id": $scope.cfg.ASMAY_Id
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
                        apiService.create("CollegeStaffAndOtherTransaction/Deletedetails_s", data).
                            then(function (promise) {
                                //if (promise.returnval == "true") {

                                //    if ($scope.cfg.ASMAY_Id === promise.fillyear[0].asmaY_Id) {
                                //    }
                                //    else {
                                //        $scope.yearlst = promise.fillyear;
                                //        $scope.cfg.ASMAY_Id = promise.fillyear[0].asmaY_Id;
                                //    }

                                //    if (autoreceipt == 1) {
                                //        $scope.showreceiptno = false;
                                //    }
                                //    else {
                                //        $scope.showreceiptno = true;
                                //    }

                                //    $scope.receiptgrid = promise.receiparraydelete;

                                //    $scope.FYP_Date = new Date();
                                //    $scope.FYP_DD_Cheque_Date = new Date();

                                //    if ($scope.FYP_Bank_Or_Cash == 'C') {
                                //        $scope.bankdetails = false;
                                //    }
                                //    else if ($scope.FYP_Bank_Or_Cash == 'B') {
                                //        $scope.bankdetails = true;
                                //    }

                                //    //configuration settings
                                //    if (promise.feeconfiglist.length > 0) {
                                //        grouporterm = promise.feeconfiglist[0].fmC_GroupOrTermFlg;

                                //        if (grouporterm == "T") {
                                //            $scope.grouportername = "Term Name"
                                //        }
                                //        else if (grouporterm == "G") {
                                //            $scope.grouportername = "Group Name"
                                //        }
                                //    }

                                //    swal('Record Deleted Successfully');
                                //    $state.reload();
                                //}
                                //else {
                                //    swal('Record cannot be deleted.Transaction has already been done for this group');
                                //}
                                //$state.reload();
                                if (promise.returnval == "Delete") {
                                    swal('Record Deleted Successfully');
                                    $state.reload();
                                }
                                else if (promise.returnval == "Cancel") {
                                    swal("Record Deletion Failed");
                                }
                                else if (promise.returnval == "Error") {
                                    swal('Kindly contact Administrator ');
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        }

        $scope.DeletRecord_o = function (paymentid, stuid) {


            var data = {
                "FYP_Id": paymentid,
                "FMOST_Id": stuid,
                "ASMAY_Id": $scope.cfg.ASMAY_Id
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
                        apiService.create("CollegeStaffAndOtherTransaction/Deletedetails_o", data).
                            then(function (promise) {

                                if (promise.returnval == "Delete") {
                                    swal('Record Deleted Successfully');
                                    $state.reload();
                                }
                                else if (promise.returnval == "Cancel") {
                                    swal("Record Deletion Failed");
                                }
                                else if (promise.returnval == "Error") {
                                    swal('Kindly contact Administrator ');
                                }
                                $state.reload();
                                //else {
                                //    swal('Record cannot be deleted.Transaction has already been done for this group');
                                //}
                                //$state.reload();
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        }

    }

})();