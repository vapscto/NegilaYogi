(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeFeePreadmissionTransaction', CollegeFeePreadmissionTransaction)

    CollegeFeePreadmissionTransaction.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache', 'blockUI', 'FormSubmitter', '$window', '$compile']
    function CollegeFeePreadmissionTransaction($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache, blockUI, FormSubmitter, $window, $compile) {

        $scope.radshow = false;

        $scope.studentsavedlist = true;
        $scope.nameonradio = "Student Name";
        var HostName = location.host;

        $scope.printreceipt = false;

        $scope.printview = true;

        $scope.submitted = false;
        $scope.fillpay = function (pacA_Id) {

            $window.location.href = 'https://' + HostName + '/#/app/prospectus/';

        }
        $scope.submitted = false;
        $scope.fillpay1 = function (pacA_Id) {

            $window.location.href = 'https://' + HostName + '/#/app/srkvsnew/';

        }

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
        $scope.disablepayable = true;
        $scope.rolenamelist = "";

        var institutionid, automanualreceiptnotranum

        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        // $scope.Mi_Id = configsettings[0].mI_Id;
        if (configsettings != null) {
            institutionid = configsettings[0].mI_Id;
        }
      
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        // var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        var transactionnumbering = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transactionnumbering.length; i++) {
            if (transactionnumbering.length > 0) {
                if (transactionnumbering[i].imN_Flag == "Online") {
                    $scope.transnumbconfigurationsettingsassign = transactionnumbering[i];
                }
            }
        }
        if (configsettings != null) {
            if (configsettings.length > 0) {
                grouporterm = configsettings[0].fmC_GroupOrTermFlg;
                autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
                receiptformat = configsettings[0].fmC_Receipt_Format;
                $scope.groupsetting = configsettings[0].fee_group_setting;
                $scope.makerandchecker = configsettings[0].FMC_MakerCheckerReqdFlg;
                // mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
            }
        }

   
        if (admfigsettings != null) {
            if (admfigsettings.length > 0) {
                var logopath = admfigsettings[0].asC_Logo_Path;
            }
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
        $scope.FYP_ReceiptDate = new Date();

        $scope.formload = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10

            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = 10;

            var data = {
                "MI_Id": 1,
                "filterinitialdata": "Preadmission",
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("CollegeFeePreadmissionTransaction/getalldetails", data).
                then(function (promise) {

                    $scope.loginid = promise.userId;
                    // $scope.rolenamelist = promise.rolename;

                    $scope.saved_staff_list = promise.receiparraydelete;

                    $scope.student_list = promise.preadmissionstudentlist;

                    // $scope.rolenamelist = promise.rolename;
                    $scope.yearlst = promise.academicyrlist;
                    $scope.cfg.ASMAY_Id = $scope.yearlst[0].asmaY_Id;

                    if (promise.transnumconfig.length > 0) {
                        automanualreceiptnotranum = promise.transnumconfig[0].imN_AutoManualFlag;
                    }

                    if (promise.feeconfiglist.length > 0) {
                        autoreceipt = promise.feeconfiglist[0].fmC_AutoReceiptFeeGroupFlag;
                    }

                    if (autoreceipt == 1) {
                        $scope.showreceiptno = false;
                    }
                    else {
                        $scope.showreceiptno = true;
                    }

                    $scope.receiptgrid = promise.receiparraydelete;
                    $scope.FYP_Date = new Date();
                    $scope.FYP_DD_Cheque_Date = new Date();
                    $scope.FYPPM_ClearanceDate = new Date();

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
                    else {
                        swal("Fee Settings yet to be configured.Kindly contact Administrator!")
                    }

                    //MB for Special
                    //$scope.special_head_list = promise.specialheadlist;
                    //$scope.special_head_details = promise.specialheaddetails;
                    //MB for Special

                })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        }

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
                if (Number(totalgrid[index].toBePaid) + Number(totalgrid[index].fsS_ConcessionAmount) + Number(totalgrid[index].fsS_WaivedAmount) <= Number(totalgrid[index].totalamount)) {
                    $scope.totalconcession = Number(totalgrid[index].fsS_ConcessionAmount);
                }
                else if (Number(totalgrid[index].toBePaid) + Number(totalgrid[index].fsS_ConcessionAmount) + Number(totalgrid[index].fsS_WaivedAmount) > Number(totalgrid[index].totalamount)) {
                    swal("Entered amount is greater than Net Amount");
                }
            }

            else if (count > 1) {

                if (Number(totalgrid[index].toBePaid) + Number(totalgrid[index].fsS_ConcessionAmount) + Number(totalgrid[index].fsS_WaivedAmount) <= Number(totalgrid[index].totalamount)) {

                    //$scope.curramount = Number($scope.curramount) - Number(totalgrid[index].fsS_ToBePaid);
                    var interconcessionamt = 0
                    angular.forEach($scope.totalgrid, function (user) {
                        if (!!user.isSelected) {
                            interconcessionamt = Number(interconcessionamt) + Number(user.fsS_ConcessionAmount);
                        }
                    })
                    $scope.totalconcession = interconcessionamt
                }

                else if (Number(totalgrid[index].toBePaid) + Number(totalgrid[index].fsS_ConcessionAmount) + Number(totalgrid[index].fsS_WaivedAmount) > Number(totalgrid[index].totalamount)) {
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
                if (Number(totalgrid[index].toBePaid) + Number(totalgrid[index].fsS_ConcessionAmount) + Number(totalgrid[index].fsS_WaivedAmount) <= Number(totalgrid[index].totalamount)) {
                    $scope.totalwaived = Number(totalgrid[index].fsS_WaivedAmount);
                }
                else if (Number(totalgrid[index].toBePaid) + Number(totalgrid[index].fsS_ConcessionAmount) + Number(totalgrid[index].fsS_WaivedAmount) > Number(totalgrid[index].totalamount)) {
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
                angular.forEach($scope.Head_Instl_list, function (student) {
                    $scope.heads1.push(student);
                });
            } else {
                angular.forEach($scope.Head_Instl_list, function (student) {
                    if (student.isSelected == true) {
                        $scope.heads1.push(student);
                    }
                });
            }
            var data = {
                "auto_receipt_flag": autoreceipt,
                head_installments: $scope.heads1,
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("CollegeFeePreadmissionTransaction/get_grp_reptno", data).
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
                            var Head_Instl_list = trp[0].total_grid;
                            var index = trp[0].in_dex;
                            var userdata = trp[0].user_data;
                            angular.forEach($scope.Head_Instl_list, function (itm) {
                                if (itm.fmH_Id == userdata.fmH_Id) {
                                    itm.isSelected = false;
                                }

                            });

                            $scope.all = $scope.Head_Instl_list.every(function (itm) {
                                return itm.isSelected;
                            });

                            if (Head_Instl_list[index].isSelected == true) {

                                $scope.totalconcession = Number($scope.totalconcession) + Number(Head_Instl_list[index].fsS_ConcessionAmount);
                                $scope.totalfine = Number($scope.totalfine) + Number(Head_Instl_list[index].fsS_FineAmount);
                                //$scope.totalfine =  Number(totalgrid[index].fsS_FineAmount);
                                $scope.curramount = Number($scope.curramount) + Number(Head_Instl_list[index].toBePaid) + Number(Head_Instl_list[index].fsS_FineAmount);
                                $scope.totalwaived = Number($scope.totalwaived) + Number(Head_Instl_list[index].fsS_WaivedAmount);
                            }
                            else if (Head_Instl_list[index].isSelected == false) {

                                $scope.totalconcession = Number($scope.totalconcession) - Number(Head_Instl_list[index].fsS_ConcessionAmount);
                                ////$scope.totalfine = Number($scope.totalfine) - Number(totalgrid[index].fsS_FineAmount);
                                $scope.totalfine = Number($scope.totalfine) - Number(Head_Instl_list[index].fsS_FineAmount);
                                // $scope.totalfine = Number($scope.totalfine) - Number(totalgrid[index].fsS_FineAmount);
                                $scope.curramount = Number($scope.curramount) - Number(Head_Instl_list[index].toBePaid) - Number(Head_Instl_list[index].fsS_FineAmount);
                                $scope.totalwaived = Number($scope.totalwaived) - Number(Head_Instl_list[index].fsS_WaivedAmount);
                            }

                            trp = [];
                        }
                        else if ($scope.all == true) {
                            //$scope.all = false;
                            $scope.disablefine = true;
                            $scope.disablenetamount = true;
                            $scope.disableconcession = true;
                            $scope.disablepayable = true;
                            var allchkdata = $scope.all;
                            var toggleStatus = $scope.all;

                            $scope.curramount = 0;
                            $scope.totalconcession = 0;
                            $scope.totalfine = 0;
                            $scope.totalwaived = 0;

                            if (allchkdata == true) {

                                for (var index = 0; index < $scope.Head_Instl_list.length; index++) {
                                    $scope.totalconcession = Number($scope.totalconcession) + Number($scope.Head_Instl_list[index].fsS_ConcessionAmount);
                                    $scope.totalfine = Number($scope.totalfine) + Number($scope.Head_Instl_list[index].fsS_FineAmount);
                                    $scope.curramount = Number($scope.curramount) + Number($scope.Head_Instl_list[index].toBePaid) + $scope.totalfine;
                                    $scope.totalwaived = Number($scope.totalwaived) + Number($scope.Head_Instl_list[index].fsS_WaivedAmount);
                                }

                            }
                            else {
                                $scope.totalconcession = 0;
                                $scope.totalfine = 0;
                                $scope.curramount = 0;
                                $scope.totalwaived = 0;
                            }


                            angular.forEach($scope.Head_Instl_list, function (itm) {
                                if (itm.fmG_Id == $scope.highestcountgid) {
                                    itm.isSelected = true;
                                }
                                else {
                                    itm.isSelected = false;
                                }

                            });

                            var payableamt = 0, concamt = 0, fneamt = 0;
                            angular.forEach($scope.Head_Instl_list, function (iitm1) {
                                if (iitm1.isSelected == true) {
                                    payableamt = payableamt + iitm1.toBePaid;
                                    concamt = concamt + iitm1.fsS_ConcessionAmount;
                                    fneamt = fneamt + iitm1.fsS_FineAmount;
                                }
                                $scope.curramount = payableamt;
                                $scope.totalconcession = concamt;
                                $scope.totalfine = fneamt;
                                $scope.FYP_Tot_Amcurramountount = $scope.curramount;

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
            apiService.getURI("FeeStaffOthersTransaction/Editdetails", orgid).
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
                "FMH_FeeName": $scope.typethird
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeStaffOthersTransaction/1", data).
                then(function (promise) {
                    $scope.thirdgrid = promise.alldata;
                    swal("searched Successfully");
                })
        }

        $scope.submitted = false;
        $scope.savedatatrans = [];

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
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
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeStaffOthersTransaction/feereceiptduplicate", data).
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
        var list_s = [];

        $scope.search_flag = false;
        $scope.search123 = "";
        var search_s = "";
        //var list_s =[];
        $scope.onselectsearch = function () {
            $scope.searchtxt = "";
            $scope.search_flag = true;
            $scope.search_string = true;
            $scope.txt = true;
        }

        $scope.ShowSearch_Report = function (val, cse) {

            if (val != "") {
                var data = {
                    "searchType": cse,
                    "searchtext": val,
                    "filterinitialdata": $scope.filterdata,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CollegeFeePreadmissionTransaction/searching_s", data).
                    then(function (promise) {

                        $scope.saved_staff_list = promise.searcharray;
                        $scope.totcountsearch = promise.staff_paid_details.length;

                        if (promise.staff_paid_details == null || promise.staff_paid_details == "") {
                            swal("Record Does Not Exist For Searched Data !!!!!!")
                        }
                    })
            }
            else {
                swal("Kindly enter text")
            }

        }

        $scope.clearsearch = function () {

            $state.reload();
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

            apiService.create("FeeStaffOthersTransaction/edittransaction", data).
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
                    else if (promise.receiparraydelete[0].fyP_Bank_Or_Cash == 'U') {
                        $scope.FYP_Bank_Or_Cash = 'U';
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
        $scope.select_emp = function (stf_id) {
            if (stf_id != undefined && stf_id != "") {
                $scope.clear_values();
                var data = {
                    "Grp_Term_flg": grouporterm,
                    "HRME_Id": $scope.HRME_Id,

                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeStaffOthersTransaction/select_emp", data).
                    then(function (promise) {

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
                    "PACA_Id": stu_id.pacA_Id,
                    "filterinitialdata": $scope.filterdata,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CollegeFeePreadmissionTransaction/select_student", data).
                    then(function (promise) {

                        if (grouporterm == 'T') {
                            $scope.term_list = promise.termlist
                        }
                        else if (grouporterm == 'G') {
                            $scope.group_list = promise.termlist
                        }
                       
                        $scope.PACA_FirstName = promise.preadmissionstudentlist[0].pacA_FirstName;
                        $scope.PACA_ApplicationNo = promise.preadmissionstudentlist[0].pacA_ApplicationNo;
                        $scope.AMCO_CourseName = promise.preadmissionstudentlist[0].amcO_CourseName;
                        $scope.AMB_BranchName = promise.preadmissionstudentlist[0].amB_BranchName;
                        $scope.AMSE_SEMName = promise.preadmissionstudentlist[0].amsE_SEMName;
                        $scope.PACA_MobileNo = promise.preadmissionstudentlist[0].pacA_MobileNo;
                        $scope.PACA_emailId = promise.preadmissionstudentlist[0].pacA_emailId;
                        $scope.PACA_FatherName = promise.preadmissionstudentlist[0].fathername;
                        $scope.pacA_RegistrationNo = promise.preadmissionstudentlist[0].pacA_RegistrationNo;
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

        $scope.onselectmodeofpayment = function (ivrmmoD_ModeOfPayment_Code) {
            var data = {
                //"modeofpayment": $scope.FYP_Bank_Or_Cash,
                "modeofpayment": ivrmmoD_ModeOfPayment_Code,
                "filterinitialdata": $scope.filterdata
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            $scope.FYP_Bank_Or_Cash = ivrmmoD_ModeOfPayment_Code;

            if ($scope.FYP_Bank_Or_Cash == 'C') {
            }
            else if ($scope.FYP_Bank_Or_Cash == 'B' || $scope.FYP_Bank_Or_Cash == 'R' || $scope.FYP_Bank_Or_Cash == 'S' || $scope.FYP_Bank_Or_Cash == 'U') {
                $scope.bankdetails = true;
            }
            else {
                $scope.groupcount = [];
                $scope.cfg.ASMAY_Id = "";
                $scope.amcsT_Id = "";
                $scope.grigview1 = false;
            }

            if ($scope.FYP_Bank_Or_Cash == 'C') {
                $scope.bankdetails = false;
            }
            else if ($scope.FYP_Bank_Or_Cash == 'B') {
                $scope.bankdetails = true;
            }
        };


        //MB for Special
        var remove_list = [];
        var ins_spe_list = [];
        //MB for Special
        $scope.togchkbx_group_term = function (option) {

            var terms_OR_groups = "";
            $scope.all = false;
            $scope.terms_groups = [];

            $scope.FYP_Tot_Amount = 0;

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


            var data = {
                "PACA_Id": $scope.pacA_Id.pacA_Id,
                "AMCO_Id": $scope.pacA_Id.pacA_Id,
                terms_groups: $scope.terms_groups,
                "Grp_Term_flg": grouporterm,
                "filterinitialdata": $scope.filterdata,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("CollegeFeePreadmissionTransaction/getgroupmappedheadsnew_st", data).
                then(function (promise) {

                    if (promise.fetchmodeofpayment.length > 0) {
                        $scope.modeofpayment = promise.fetchmodeofpayment;
                    }

                    if (promise.bankname.length > 0) {
                        $scope.bankmaster = promise.bankname;
                    }

                    if (promise.mapped_hds_ins.length > 0) {
                        $scope.grigview1 = true;
                        $scope.highestcountgid = promise.validationgroupid;
                        if (promise.feepaiddetails.length > 0) {
                            angular.forEach(promise.mapped_hds_ins, function (value) {
                                angular.forEach(promise.feepaiddetails, function (value1) {
                                    if (value1.fcmaS_Id == value.fcmaS_Id) {
                                        //value.toBePaid = Number(value.fcsS_ToBePaid) - Number(value1.ftcP_PaidAmount);
                                        value.toBePaid = Number(value.toBePaid) - Number(value1.ftcP_PaidAmount);
                                    }

                                })

                            })
                        }

                        $scope.Head_Instl_list = promise.mapped_hds_ins;

                        var addfinetonetamount, addnetamount, totalpayableamount;

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

                            cur_yr_chgs = cur_yr_chgs + value.currentYrCharges;
                            to_be_paid = to_be_paid + value.toBePaid;
                        })

                        $scope.totalfee = cur_yr_chgs;
                        $scope.currbalance = to_be_paid;

                        //MB for Special

                        $scope.temp_Head_Instl_list = [];
                        angular.forEach($scope.Head_Instl_list, function (uy) {
                            uy.Head_Flag = 'H';
                            $scope.temp_Head_Instl_list.push(uy);
                        })
                        remove_list = [];
                        ins_spe_list = [];
                        angular.forEach(promise.instalspecial, function (ins) {
                            var special_list = [];
                            angular.forEach($scope.special_head_list, function (op1) {
                                var spe_ind_list = [];
                                angular.forEach($scope.special_head_details, function (op2) {
                                    if (op1.fmsfH_Id == op2.fmsfH_Id) {
                                        angular.forEach($scope.Head_Instl_list, function (op_m) {
                                            if (op_m.fmH_Id == op2.fmH_ID && op_m.ftI_Id == ins.ftI_Id) {
                                                spe_ind_list.push(op_m);
                                                remove_list.push(op_m);
                                            }
                                        })
                                    }

                                })
                                if (spe_ind_list.length > 0) {
                                    special_list.push({ sp_id: op1.fmsfH_Id, sp_name: op1.fmsfH_Name, sp_ind_list: spe_ind_list });
                                }
                            })
                            if (special_list.length > 0) {
                                ins_spe_list.push({ ftI_Id: ins.ftI_Id, ftI_Name: ins.ftI_Name, sp_list: special_list });
                            }
                        })

                        if (ins_spe_list.length > 0) {
                            angular.forEach(remove_list, function (ma1) {
                                $scope.temp_Head_Instl_list.splice($scope.temp_Head_Instl_list.indexOf(ma1), 1);
                            })
                            //$scope.Head_Instl_list = $scope.temp_Head_Instl_list;
                            //angular.forEach($scope.temp_Head_Instl_list, function (r) {
                            //    r.Head_Flag = 'H';
                            //})
                            angular.forEach(ins_spe_list, function (a1) {

                                angular.forEach(a1.sp_list, function (a2) {
                                    var currentYrCharges = 0;
                                    var totalCharges = 0;
                                    var concessionAmount = 0;
                                    var fineAmount = 0;
                                    var toBePaid = 0;
                                    var netAmount = 0;
                                    var fmG_Id = 0;
                                    var fmG_GroupName = '';
                                    var not_cnt = 0;
                                    angular.forEach(a2.sp_ind_list, function (a3) {
                                        if (fmG_Id == 0) {
                                            fmG_Id = a3.fmG_Id;
                                            fmG_GroupName = a3.fmG_GroupName;
                                        }
                                        else {
                                            if (fmG_Id != a3.fmG_Id) {
                                                not_cnt += 1;
                                            }
                                        }
                                        currentYrCharges += a3.currentYrCharges;
                                        totalCharges += a3.totalCharges;
                                        concessionAmount += a3.concessionAmount;
                                        fineAmount += a3.fineAmount;
                                        toBePaid += a3.toBePaid;
                                        netAmount += a3.netAmount;
                                    })
                                    if (not_cnt == 0) {
                                        $scope.temp_Head_Instl_list.push({ fmG_Id: fmG_Id, fmG_GroupName: 'SH_' + fmG_GroupName, fmH_Id: a2.sp_id, fmH_FeeName: a2.sp_name, ftI_Id: a1.ftI_Id, ftI_Name: a1.ftI_Name, currentYrCharges: currentYrCharges, totalCharges: totalCharges, concessionAmount: concessionAmount, fineAmount: fineAmount, toBePaid: toBePaid, netAmount: netAmount, Head_Flag: 'SH' });
                                    }
                                    else if (not_cnt > 0) {
                                        $scope.temp_Head_Instl_list.push({ fmG_Id: 0, fmG_GroupName: 'Special_Head', fmH_Id: a2.sp_id, fmH_FeeName: a2.sp_name, ftI_Id: a1.ftI_Id, ftI_Name: a1.ftI_Name, currentYrCharges: currentYrCharges, totalCharges: totalCharges, concessionAmount: concessionAmount, fineAmount: fineAmount, toBePaid: toBePaid, netAmount: netAmount, Head_Flag: 'SH' });
                                    }

                                })
                            })
                            $scope.Head_Instl_list = $scope.temp_Head_Instl_list;
                        }
                    }
                    else {

                        if ($scope.terms_groups.length > 0) {
                            swal("Student has paid amount for Selected " + terms_OR_groups);
                        }
                        else if ($scope.terms_groups.length == 0) {
                            $scope.Head_Instl_list = promise.mapped_hds_ins;
                        }

                        //swal("Student has paid amount for that Group");
                    }
                })
            //   }
        };

        $scope.tobepaidamt_st = function (totalgrid, index) {
            var count = 0, intertobepaidamt = 0;

            angular.forEach(totalgrid, function (user) {
                if (!!user.isSelected) {
                    count = count + 1
                }
            })

            if (count <= 1) {

                if (Number(totalgrid[index].totalCharges) >= Number(totalgrid[index].toBePaid)) {
                    $scope.curramount = Number(totalgrid[index].toBePaid);
                }
                else if ((Number(totalgrid[index].totalCharges) <= Number(totalgrid[index].toBePaid)) && Number(totalgrid[index].totalCharges) > 0) {
                    swal("Entered Amount is greater than Netamount");
                    totalgrid[index].toBePaid = Number(totalgrid[index].totalCharges);
                    $scope.curramount = Number(totalgrid[index].totalCharges);
                }
                else if (Number(totalgrid[index].totalCharges) == 0) {
                    $scope.curramount = Number(totalgrid[index].toBePaid);
                }
            }
            else if (count > 1) {

                if (Number(totalgrid[index].totalCharges) >= Number(totalgrid[index].toBePaid)) {

                    angular.forEach(totalgrid, function (user) {
                        if (!!user.isSelected) {

                            intertobepaidamt = Number(intertobepaidamt) + Number(user.toBePaid);
                        }
                    })

                    $scope.curramount = intertobepaidamt;
                }

                else if ((Number(totalgrid[index].totalCharges) <= Number(totalgrid[index].toBePaid)) && Number(totalgrid[index].totalCharges)) {

                    swal("Entered Amount is greater than Netamount");

                    totalgrid[index].toBePaid = Number(totalgrid[index].totalCharges);

                    angular.forEach(totalgrid, function (user) {
                        if (!!user.isSelected) {

                            intertobepaidamt = Number(intertobepaidamt) + Number(user.toBePaid);
                        }
                    })

                    $scope.curramount = intertobepaidamt;
                }

                else if (Number(totalgrid[index].totalCharges) == 0) {

                    angular.forEach(totalgrid, function (user) {
                        if (!!user.isSelected) {

                            intertobepaidamt = Number(intertobepaidamt) + Number(user.toBePaid);
                        }
                    })

                    $scope.curramount = intertobepaidamt;
                }
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
            $scope.disablepayable = true;

            $scope.all = $scope.Head_Instl_list.every(function (itm) {
                return itm.isSelected;
            });

            if (Head_Instl_list[index].isSelected == true) {
                //$scope.totalconcession = Number($scope.totalconcession) + Number(Head_Instl_list[index].fsS_ConcessionAmount);
                //$scope.totalfine = Number($scope.totalfine) + Number(Head_Instl_list[index].fsS_FineAmount);
                $scope.curramount = Number($scope.curramount) + Number(Head_Instl_list[index].toBePaid)
                // $scope.totalwaived = Number($scope.totalwaived) + Number(Head_Instl_list[index].fsS_WaivedAmount);
            }
            else if (Head_Instl_list[index].isSelected == false) {
                //$scope.totalconcession = Number($scope.totalconcession) - Number(Head_Instl_list[index].fsS_ConcessionAmount);
                //$scope.totalfine = Number($scope.totalfine) - Number(totalgrid[index].fsS_FineAmount);
                // $scope.totalfine = Number($scope.totalfine) - Number(Head_Instl_list[index].fsS_FineAmount);
                $scope.curramount = Number($scope.curramount) - Number(Head_Instl_list[index].toBePaid)
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
            $scope.disablepayable = true;
            var toggleStatus = $scope.all;

            $scope.curramount = 0;
            $scope.FYP_Tot_Concession_Amt = 0;
            $scope.FYP_Tot_Fine_Amt = 0;
            $scope.totalwaived = 0;

            angular.forEach($scope.Head_Instl_list, function (itm) {
                itm.isSelected = toggleStatus;
            });
            if (allchkdata == true) {

                // $scope.highestcountgid = 21;
                //angular.forEach($scope.Head_Instl_list, function (itm) {
                //    if (itm.fmG_Id == $scope.highestcountgid) {
                //        itm.isSelected = true;
                //    }
                //    else {
                //        itm.isSelected = false;
                //    }
                //});

                for (var index = 0; index < $scope.Head_Instl_list.length; index++) {

                    if ($scope.Head_Instl_list[index].isSelected) {
                        $scope.curramount = Number($scope.curramount) + Number($scope.Head_Instl_list[index].toBePaid);
                    }
                    // $scope.FYP_Tot_Concession_Amt = Number($scope.FYP_Tot_Concession_Amt) + Number($scope.Head_Instl_list[index].concessionAmount);
                    //  $scope.FYP_Tot_Fine_Amt = Number($scope.FYP_Tot_Fine_Amt) + Number($scope.Head_Instl_list[index].fineAmount);
                    // $scope.curramount = Number($scope.curramount) + Number($scope.Head_Instl_list[index].toBePaid) //+ Number($scope.Head_Instl_list[index].fineAmount);
                    //   $scope.totalwaived = Number($scope.totalwaived) + Number($scope.Head_Instl_list[index].fsS_WaivedAmount);
                }
            }

            else {
                $scope.FYP_Tot_Concession_Amt = 0;
                $scope.FYP_Tot_Fine_Amt = 0;
                $scope.curramount = 0;
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
            var count = 0;
            var save_hes_insts = [];
            //MB For Special
            if (ins_spe_list.length == 0 && remove_list.length == 0) {
                angular.forEach(Head_Instl_list, function (opq) {
                    if (opq.isSelected) {
                        count += 1;
                        //  opq.netAmount = Number(opq.netAmount);
                        save_hes_insts.push(opq);
                    }
                })
            }
            else if (ins_spe_list.length > 0 && remove_list.length > 0) {
                angular.forEach(Head_Instl_list, function (opq) {
                    if (opq.isSelected) {
                        if (opq.Head_Flag == 'H') {
                            count += 1;
                            //  opq.netAmount = Number(opq.netAmount);
                            save_hes_insts.push(opq);
                        }
                        else if (opq.Head_Flag == 'SH') {
                            angular.forEach(ins_spe_list, function (s) {
                                if (s.ftI_Id == opq.ftI_Id) {
                                    angular.forEach(s.sp_list, function (s1) {
                                        if (s1.sp_id == opq.fmH_Id) {
                                            var toBePaid = 0;
                                            angular.forEach(s1.sp_ind_list, function (s2) {
                                                //count += 1;
                                                //save_hes_insts.push(s2);
                                                toBePaid += Number(s2.toBePaid);
                                            })
                                            if (toBePaid == Number(opq.toBePaid)) {
                                                angular.forEach(s1.sp_ind_list, function (s2) {
                                                    count += 1;
                                                    save_hes_insts.push(s2);
                                                })
                                            }
                                            else if (toBePaid > Number(opq.toBePaid)) {
                                                //var keepGoing = true;
                                                //angular.forEach([0,1,2], function(count){
                                                //    if(keepGoing) {
                                                //        if(count == 1){
                                                //            keepGoing = false;
                                                //        }
                                                //    }
                                                //});
                                                var keepGoing = true;
                                                angular.forEach(s1.sp_ind_list, function (s2) {
                                                    if (keepGoing) {
                                                        if (Number(opq.toBePaid) >= Number(s2.toBePaid)) {
                                                            count += 1;
                                                            save_hes_insts.push(s2);
                                                            opq.toBePaid = (Number(opq.toBePaid) - Number(s2.toBePaid));
                                                        }
                                                        else if (Number(opq.toBePaid) < Number(s2.toBePaid)) {
                                                            s2.toBePaid = Number(opq.toBePaid);
                                                            count += 1;
                                                            save_hes_insts.push(s2);
                                                            opq.toBePaid = (Number(opq.toBePaid) - Number(s2.toBePaid));
                                                        }
                                                        if (Number(opq.toBePaid) == 0) {
                                                            keepGoing = false;
                                                        }
                                                    }

                                                })
                                            }

                                        }

                                    })
                                }

                            })
                        }
                    }
                })
            }
            //MB For Special

            if (count > 0) {
                if ($scope.curramount > 0) {
                    if ($scope.myForm.$valid) {

                        if ($scope.FYP_Remarks == undefined || $scope.FYP_Remarks == null) {
                            $scope.FYP_Remarks = "";
                        }
                        if ($scope.FYP_Bank_Or_Cash == 'C') {
                            $scope.FYP_Bank_Name = "";
                            $scope.FYP_DD_Cheque_No = "";
                        }
                        var data = {
                            "FYP_Id": $scope.FYP_Id,
                            "ASMAY_Id": $scope.cfg.ASMAY_Id,
                            "FYP_ReceiptNo": $scope.FYP_Receipt_No,
                            //"FYPPM_BankName": $scope.FYP_Bank_Name,
                            "FYPPM_BankName": $scope.FMBANK_Id,
                            "FYP_TransactionTypeFlag": $scope.FYP_Bank_Or_Cash,
                            "FYPPM_DDChequeNo": $scope.FYP_DD_Cheque_No,
                            "FYPPM_DDChequeDate": new Date($scope.FYP_DD_Cheque_Date).toDateString(),
                            "FYPPM_ClearanceDate": new Date($scope.FYPPM_ClearanceDate).toDateString(),
                            "FYP_ReceiptDate": new Date($scope.FYP_Date).toDateString(),
                            "FYP_TotalPaidAmount": $scope.curramount,
                            "FYP_TotalFineAmount": $scope.FYP_Tot_Fine_Amt,
                            //"FYP_Tot_Concession_Amt": $scope.FYP_Tot_Concession_Amt,
                            "FYP_Remarks": $scope.FYP_Remarks,
                            head_installments: save_hes_insts,
                            "PACA_Id": $scope.pacA_Id.pacA_Id,
                            "auto_receipt_flag": autoreceipt,
                            "automanualreceiptno": automanualreceiptnotranum,
                            transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                            "filterinitialdata": $scope.filterdata,
                        }

                        var config = {
                            headers: {
                                'Content-Type': 'application/json;'
                            }
                        }

                        apiService.create("CollegeFeePreadmissionTransaction/savedata_st", data).
                            then(function (promise) {

                                if (promise.returnval == "Cancel") {
                                    swal("Transaction Failed");
                                }

                                else if (promise.returnval == "Save") {
                                    swal('Transaction Saved Successfully');
                                }
                                else if (promise.returnval == "Error") {
                                    swal('Kindly contact Administrator ');
                                }
                                else {
                                    swal(promise.returnval);
                                }

                                $state.reload();

                            })

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


        $scope.showmodaldetails_s = function (fypid, studentid) {

            $scope.ASMCL_ClassNamefirst = "";
            $scope.AMST_FirstName = "";
            $scope.curdate = "";
            $scope.asmaY_Year = "";
            $scope.AMST_FatherName = "";
            $scope.AMST_MotherName = "";
            $scope.receiptno = "";
            $scope.FMCC_ConcessionName = "";

            var data = {
                "FYP_Id": fypid,
                "PACA_Id": studentid,
                "minstall": 0,
                "filterinitialdata": $scope.filterdata,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("CollegeFeePreadmissionTransaction/printreceipt_s", data).
                then(function (promise) {
                    $scope.colspan = 3;

                    if (promise.receiptformathead.length > 0) {
                        $scope.showdetailsreceipt = promise.receiptformathead
                    }

                    if (promise.receiptdetails != null) {
                        if (promise.receiptdetails.length > 0) {
                            $scope.staff_fee_receipt_details = promise.receiptdetails;
                        }
                    }

                    $scope.username = promise.username;

                    if (promise.approvedbyname != undefined) {
                        if (promise.approvedbyname.length > 0) {
                            $scope.approvedby = promise.approvedbyname[0].EmpName;
                            $scope.feerectype = "FEE"
                        }
                        else {
                            $scope.approvedby = "";
                            $scope.feerectype = "PROVISIONAL"
                        }
                    }
                    else {
                        $scope.approvedby = "";
                        $scope.feerectype = "PROVISIONAL"
                    }

                    if (promise.fillstudentviewdetails != null && promise.fillstudentviewdetails.length > 0) {
                        $scope.FYP_Receipt_No = promise.fillstudentviewdetails[0].fyP_Receipt_No;
                        $scope.FYP_ReceiptDate = $filter('date')(promise.fillstudentviewdetails[0].fyP_ReceiptDate, "dd-MM-yyyy");
                        $scope.PACA_ApplicationNo = promise.fillstudentviewdetails[0].pacA_ApplicationNo;
                        $scope.ASMAY_Year = promise.fillstudentviewdetails[0].asmaY_Year;
                        $scope.PACA_FirstName = promise.fillstudentviewdetails[0].pacA_FirstName;
                        $scope.AMCO_CourseName = promise.fillstudentviewdetails[0].amcO_CourseName;
                        $scope.AMB_BranchName = promise.fillstudentviewdetails[0].amB_BranchName;
                        $scope.AMSE_SEMName = promise.fillstudentviewdetails[0].amsE_SEMName;
                        $scope.PACA_RegistrationNo = promise.fillstudentviewdetails[0].pacA_RegistrationNo;
                        $scope.FYP_Remarks = promise.fillstudentviewdetails[0].fyP_Remarks;
                        
                        //added//
                        $scope.ASMCL_ClassNamefirst = promise.fillstudentviewdetails[0].amsE_SEMName;
                        $scope.asmaY_Year = promise.fillstudentviewdetails[0].asmaY_Year;
                        $scope.receiptno = promise.fillstudentviewdetails[0].fyP_Receipt_No;
                        $scope.pasR_FirstNameview = promise.fillstudentviewdetails[0].pacA_FirstName;
                        $scope.fathername = promise.fillstudentviewdetails[0].fathername;
                        $scope.totalpaidamount = promise.fillstudentviewdetails[0].fyP_TotalPaidAmount;
                        $scope.AMCST_NEETRN = promise.fillstudentviewdetails[0].amcsT_NEETRN;
                        $scope.currdatee = $filter('date')(promise.fillstudentviewdetails[0].fyP_ReceiptDate, "dd-MM-yyyy");
                        $scope.FYPPM_ClearanceDate = $filter('date')(promise.fillstudentviewdetails[0].fyppM_ClearanceDate, "dd-MM-yyyy");
                        //added//
                    }

                    var modeofpayment = "",modedetails="";
                    if (promise.fillstudentviewdetails[0].fyP_TransactionTypeFlag == 'C') {
                        $scope.modeofpayment = "Cash";
                        promise.fillstudentviewdetails[0].fyppM_ClearanceDate = "";
                        $scope.modedetails = modeofpayment;
                    }
                    else if (promise.fillstudentviewdetails[0].fyP_TransactionTypeFlag == 'B') {
                        $scope.moddet = promise.fillstudentviewdetails[0].fyP_TransactionTypeFlag;
                        $scope.modeofpayment = "Bank";
                        $scope.FYP_DD_Cheque_No = promise.fillstudentviewdetails[0].fyP_DD_Cheque_No;
                        $scope.FYP_DD_Cheque_Date = promise.fillstudentviewdetails[0].fyP_DD_Cheque_Date;
                        $scope.FYP_Bank_Name = promise.fillstudentviewdetails[0].fyP_Bank_Name;
                    }
                    else if (promise.fillstudentviewdetails[0].fyP_TransactionTypeFlag == 'O') {
                        modeofpayment = "Online";
                        $scope.modedetails = modeofpayment;
                        $scope.FYPPM_Transaction_Id = promise.fillstudentviewdetails[0].fyppM_Transaction_Id;
                        $scope.FYPPM_PaymentReference_Id = promise.fillstudentviewdetails[0].fyppM_PaymentReference_Id;
                    }
                    else if (promise.fillstudentviewdetails[0].fyP_TransactionTypeFlag == 'S') {
                        modeofpayment = "Swipe";
                        $scope.modedetails = modeofpayment;
                        $scope.FYP_DD_Cheque_No = promise.fillstudentviewdetails[0].fyP_DD_Cheque_No;
                        $scope.FYP_DD_Cheque_Date = promise.fillstudentviewdetails[0].fyP_DD_Cheque_Date;
                        $scope.FYP_Bank_Name = promise.fillstudentviewdetails[0].fyP_Bank_Name;
                    }
                    else if (promise.fillstudentviewdetails[0].fyP_TransactionTypeFlag == 'R') {
                        modeofpayment = "RTGS/NEFT";
                        $scope.modedetails = modeofpayment;
                        $scope.FYP_DD_Cheque_No = promise.fillstudentviewdetails[0].fyP_DD_Cheque_No;
                        $scope.FYP_DD_Cheque_Date = promise.fillstudentviewdetails[0].fyP_DD_Cheque_Date;
                        $scope.FYP_Bank_Name = promise.fillstudentviewdetails[0].fyP_Bank_Name;
                    }

                    $scope.paymenrgrid = promise.fillstudentviewdetails;

                    var totpaidamt = 0, amountinwords = 0;
                    angular.forEach(promise.receiptformathead, function (e) {
                        totpaidamt = Number(totpaidamt) + e.ftcP_PaidAmount;
                    })

                    amountinwords = Number(totpaidamt);
                    $scope.atotA = Number(totpaidamt);
                    $scope.totalpaidamount = amountinwords;
                    $scope.words = $scope.amountinwords(amountinwords);

                    $scope.htmldata = promise.htmldata;

                    var e1 = angular.element(document.getElementById("test"));
                    $compile(e1.html(promise.htmldata))(($scope));
                   
                    if ($scope.htmldata != '') {
                        $("#myModal1").modal();
                    }
                    else {
                        $("#myModal_s").modal();
                    }

                })
        }

        $scope.printData_s = function () {
            var innerContents = document.getElementById("printmodal_s").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/JSHS/feerecieptPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
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

        $scope.DeletRecord_s = function (paymentid, studid) {


            var data = {
                "FYP_Id": paymentid,
                "PACA_Id": studid,
                "filterinitialdata": $scope.filterdata,
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
                        apiService.create("CollegeFeePreadmissionTransaction/Deletedetails_s", data).
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
                "FMOST_Id": stuid
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
                        apiService.create("FeeStaffOthersTransaction/Deletedetails_o", data).
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



        $scope.showprintdata = function (stuid) {

            apiService.getURI("StudentApplication/getprintdata", $scope.pacA_Id.pacA_Id).then(function (promise) {
                $scope.htmldata = promise.htmldata;
                $scope.serilano = promise.studentReg_DTObj[0].pacA_Id;

                $scope.pacA_Id = promise.studentReg_DTObj[0].pasR_RegistrationNo;
                $scope.PASR_FirstName = promise.studentReg_DTObj[0].pasR_FirstName == null ? "" : promise.studentReg_DTObj[0].pasR_FirstName;
                $scope.PASR_MiddleName = promise.studentReg_DTObj[0].pasR_MiddleName == null ? "" : promise.studentReg_DTObj[0].pasR_MiddleName;
                $scope.PASR_LastName = promise.studentReg_DTObj[0].pasR_LastName == null ? "" : promise.studentReg_DTObj[0].pasR_LastName;
                
                $scope.PASR_Date = new Date(promise.studentReg_DTObj[0].pasR_Date);
                $scope.PASR_RegistrationNo = promise.studentReg_DTObj[0].pasR_RegistrationNo == null ? "" : promise.studentReg_DTObj[0].pasR_RegistrationNo;
                //AMC ID
                $scope.PASR_Sex = promise.studentReg_DTObj[0].pasR_Sex == null ? "" : promise.studentReg_DTObj[0].pasR_Sex;
                $scope.PASR_DOB = new Date(promise.studentReg_DTObj[0].pasR_DOB);

                var doob = promise.studentReg_DTObj[0].pasR_DOB;

                var doobyr = doob.substring(0, 4);
                var doobmnth = doob.substring(5, 7);
                var doobdays = doob.substring(8, 10);

                $scope.b1 = doobdays.substring(0, 1);
                $scope.b2 = doobdays.substring(1, 2);


                $scope.b3 = doobmnth.substring(0, 1);
                $scope.b4 = doobmnth.substring(1, 2);

                $scope.b5 = doobyr.substring(0, 1);
                $scope.b6 = doobyr.substring(1, 2);
                $scope.b7 = doobyr.substring(2, 3);
                $scope.b8 = doobyr.substring(3, 4);

                $scope.PASR_Age = promise.studentReg_DTObj[0].pasR_Age;
                $scope.PASR_DOBWords = promise.studentReg_DTObj[0].pasR_DOBWords;
                $scope.asmcL_ClassName = promise.studentClass.length > 0 ? promise.studentClass[0].asmcL_ClassName : "";
                $scope.PASR_BloodGroup = promise.studentReg_DTObj[0].pasR_BloodGroup == null ? "" : promise.studentReg_DTObj[0].pasR_BloodGroup;;
                $scope.PASR_Emisno = promise.studentReg_DTObj[0].pasR_Emisno == null ? "" : promise.studentReg_DTObj[0].pasR_Emisno;;
                $scope.PASR_Boarding = promise.studentReg_DTObj[0].pasR_Boarding == null ? "" : promise.studentReg_DTObj[0].pasR_Boarding;;
                $scope.PASR_MotherTongue = promise.studentReg_DTObj[0].pasR_MotherTongue == null ? "" : promise.studentReg_DTObj[0].pasR_MotherTongue;;
                $scope.religionname = promise.studentReligion.length > 0 ? promise.studentReligion[0].ivrmmR_Name : "";//
                if (promise.concessioncategory.length > 0) {
                    $scope.concessioncat = promise.concessioncategory.length > 0 ? promise.concessioncategory[0].fmmC_ConcessionName : "";
                }

                $scope.IMCC_CategoryName = promise.studentcastecategory.length > 0 ? promise.studentcastecategory[0].imcC_CategoryName : "";
                $scope.IMC_CasteName = promise.studentcaste.length > 0 ? promise.studentcaste[0].imC_CasteName : "";

                $scope.PASR_PerStreet = promise.studentReg_DTObj[0].pasR_PerStreet == null ? "" : promise.studentReg_DTObj[0].pasR_PerStreet;
                $scope.PASR_PerArea = promise.studentReg_DTObj[0].pasR_PerArea == null ? "" : promise.studentReg_DTObj[0].pasR_PerArea;
                $scope.PASR_PerCity = promise.studentReg_DTObj[0].pasR_PerCity == null ? "" : promise.studentReg_DTObj[0].pasR_PerCity;
                $scope.PASR_PerStaten = promise.studentperstate.length > 0 ? promise.studentperstate[0].pasR_PerStaten : "";
                $scope.PASR_PerCountryn = promise.studentpercountry.length > 0 ? promise.studentpercountry[0].pasR_PerCountryn : "";
                $scope.PASR_PerPincode = promise.studentReg_DTObj[0].pasR_PerPincode != 0 ? promise.studentReg_DTObj[0].pasR_PerPincode : "";


                $scope.PASR_ConStreet = promise.studentReg_DTObj[0].pasR_ConStreet == null ? "" : promise.studentReg_DTObj[0].pasR_ConStreet;
                $scope.PASR_ConArea = promise.studentReg_DTObj[0].pasR_ConArea == null ? "" : promise.studentReg_DTObj[0].pasR_ConArea;
                $scope.PASR_ConCity = promise.studentReg_DTObj[0].pasR_ConCity == null ? "" : promise.studentReg_DTObj[0].pasR_ConCity;
                $scope.PASR_ConStaten = promise.studentconstate.length > 0 ? promise.studentconstate[0].pasR_ConStaten : "";
                $scope.PASR_ConCountryn = promise.studentconcountry.length > 0 ? promise.studentconcountry[0].pasR_ConCountryn : "";
                $scope.PASR_ConPincode = promise.studentReg_DTObj[0].pasR_ConPincode != 0 ? promise.studentReg_DTObj[0].pasR_ConPincode : "";

                $scope.PASR_AadharNo = promise.studentReg_DTObj[0].pasR_AadharNo != 0 ? promise.studentReg_DTObj[0].pasR_AadharNo : "";
                $scope.PASR_MobileNo = promise.studentReg_DTObj[0].pasR_MobileNo != 0 ? promise.studentReg_DTObj[0].pasR_MobileNo : "";
                $scope.PASR_emailId = promise.studentReg_DTObj[0].pasR_emailId == null ? "" : promise.studentReg_DTObj[0].pasR_emailId;
                $scope.PASR_MaritalStatus = promise.studentReg_DTObj[0].pasR_MaritalStatus == null ? "" : promise.studentReg_DTObj[0].pasR_MaritalStatus;

                $scope.PASR_FatherAliveFlag = promise.studentReg_DTObj[0].pasR_FatherAliveFlag == 0 ? "No" : "Yes";
                $scope.PASR_FatherName = promise.studentReg_DTObj[0].pasR_FatherName == null ? "" : promise.studentReg_DTObj[0].pasR_FatherName;
                $scope.PASR_FatherAadharNo = promise.studentReg_DTObj[0].pasR_FatherAadharNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_FatherAadharNo;
                $scope.PASR_FatherSurname = promise.studentReg_DTObj[0].pasR_FatherSurname == null ? "" : promise.studentReg_DTObj[0].pasR_FatherSurname;
                $scope.PASR_FatherEducation = promise.studentReg_DTObj[0].pasR_FatherEducation == null ? "" : promise.studentReg_DTObj[0].pasR_FatherEducation;
                $scope.PASR_FatherOccupation = promise.studentReg_DTObj[0].pasR_FatherOccupation == null ? "" : promise.studentReg_DTObj[0].pasR_FatherOccupation;
                $scope.PASR_FatherDesignation = promise.studentReg_DTObj[0].pasR_FatherDesignation == null ? "" : promise.studentReg_DTObj[0].pasR_FatherDesignation;
                $scope.PASR_FatherIncome = promise.studentReg_DTObj[0].pasR_FatherIncome;
                $scope.PASR_FatherMobleNo = promise.studentReg_DTObj[0].pasR_FatherMobleNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_FatherMobleNo;
                $scope.PASR_FatheremailId = promise.studentReg_DTObj[0].pasR_FatheremailId == null ? "" : promise.studentReg_DTObj[0].pasR_FatheremailId;

                $scope.syllabussname = promise.sylabusss.length > 0 ? promise.sylabusss[0].imC_CasteName : "";
                $scope.PASR_FatherReligion = promise.fatherreligion.length > 0 ? promise.fatherreligion[0].imC_CasteName : "";
                $scope.PASR_FatherCaste = promise.fathercaste.length > 0 ? promise.fathercaste[0].imC_CasteName : "";;

                //$scope.fatherSubCaste_Id = promise.fathersubcaste.length > 0 ? promise.fathersubcaste[0].imC_CasteName : "";;
                //$scope.motherSubCaste_Id = promise.mothersubcaste.length > 0 ? promise.mothersubcaste[0].imC_CasteName : "";;
                //$scope.SubCaste_Id = promise.subcaste.length > 0 ? promise.subcaste[0].imC_CasteName : "";;
                //$scope.reg.fatherSubCaste_Id = promise.studentReg_DTObj[0].pasR_Fathersubcaste;
                //$scope.reg.motherSubCaste_Id = promise.studentReg_DTObj[0].pasR_Mothersubcatse;
                //$scope.reg.SubCaste_Id = promise.studentReg_DTObj[0].pasR_Subcaste;
                $scope.fatherSubCaste_Id = promise.studentReg_DTObj[0].pasR_Fathersubcaste == null ? "" : promise.studentReg_DTObj[0].pasR_Fathersubcaste;
                $scope.SubCaste_Id = promise.studentReg_DTObj[0].pasR_Subcaste == null ? "" : promise.studentReg_DTObj[0].pasR_Subcaste;
                $scope.motherSubCaste_Id = promise.studentReg_DTObj[0].pasR_Mothersubcatse == null ? "" : promise.studentReg_DTObj[0].pasR_Mothersubcatse;

                $scope.PASR_FatherTribe = promise.studentReg_DTObj[0].pasR_FatherTribe;
                $scope.PASR_Tribe = promise.studentReg_DTObj[0].pasR_Tribe;
                $scope.PASR_MotherReligion = promise.motherreligion.length > 0 ? promise.motherreligion[0].imC_CasteName : "";
                $scope.PASR_MotherCaste = promise.mothercaste.length > 0 ? promise.mothercaste[0].imC_CasteName : "";;
                $scope.PASR_MotherTribe = promise.studentReg_DTObj[0].pasR_MotherTribe;

                $scope.PASR_FirstLanguage = promise.studentReg_DTObj[0].pasR_FirstLanguage == null ? "" : promise.studentReg_DTObj[0].pasR_FirstLanguage;
                $scope.PASR_Thirdlanguage = promise.studentReg_DTObj[0].pasR_Thirdlanguage == null ? "" : promise.studentReg_DTObj[0].pasR_Thirdlanguage;
                $scope.PASR_SecondLanguage = promise.studentReg_DTObj[0].pasR_SecondLanguage == null ? "" : promise.studentReg_DTObj[0].pasR_SecondLanguage;
                //   $scope.PASR_Thirdlanguage = promise.studentReg_DTObj[0].pasR_Thirdlanguage == null ? "" : promise.studentReg_DTObj[0].pasR_Thirdlanguage;


                $scope.PASR_MotherAliveFlag = promise.studentReg_DTObj[0].pasR_MotherAliveFlag == 0 ? "No" : "Yes";
                $scope.PASR_MotherName = promise.studentReg_DTObj[0].pasR_MotherName == null ? "" : promise.studentReg_DTObj[0].pasR_MotherName;
                $scope.PASR_MotherAadharNo = promise.studentReg_DTObj[0].pasR_MotherAadharNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_MotherAadharNo;
                $scope.PASR_MotherSurname = promise.studentReg_DTObj[0].pasR_MotherSurname == null ? "" : promise.studentReg_DTObj[0].pasR_MotherSurname;
                $scope.PASR_MotherEducation = promise.studentReg_DTObj[0].pasR_MotherEducation == null ? "" : promise.studentReg_DTObj[0].pasR_MotherEducation;
                $scope.PASR_MotherOccupation = promise.studentReg_DTObj[0].pasR_MotherOccupation == null ? "" : promise.studentReg_DTObj[0].pasR_MotherOccupation;
                $scope.PASR_MotherDesignation = promise.studentReg_DTObj[0].pasR_MotherDesignation == null ? "" : promise.studentReg_DTObj[0].pasR_MotherDesignation;
                $scope.PASR_MotherIncome = promise.studentReg_DTObj[0].pasR_MotherIncome;
                $scope.PASR_MotherMobleNo = promise.studentReg_DTObj[0].pasR_MotherMobleNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_MotherMobleNo;
                $scope.PASR_MotheremailId = promise.studentReg_DTObj[0].pasR_MotheremailId == null ? "" : promise.studentReg_DTObj[0].pasR_MotheremailId;


                $scope.PASR_ChurchAddress = promise.studentReg_DTObj[0].pasR_ChurchAddress == 0 ? "" : promise.studentReg_DTObj[0].pasR_ChurchAddress;
                $scope.PASR_Churchname = promise.studentReg_DTObj[0].pasR_ChurchName == 0 ? "" : promise.studentReg_DTObj[0].pasR_ChurchName;

                $scope.PASR_FatherOfficePhNo = promise.studentReg_DTObj[0].pasR_FatherOfficePhNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_FatherOfficePhNo; $scope.PASR_FatherHomePhNo = promise.studentReg_DTObj[0].pasR_FatherHomePhNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_FatherHomePhNo; $scope.PASR_MotherOfficePhNo = promise.studentReg_DTObj[0].pasR_MotherOfficePhNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_MotherOfficePhNo;
                $scope.PASR_MotherHomePhNo = promise.studentReg_DTObj[0].pasR_MotherHomePhNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_MotherHomePhNo;

                $scope.PASR_FatherPassingYear = promise.studentReg_DTObj[0].pasR_FatherPassingYear == 0 ? "" : promise.studentReg_DTObj[0].pasR_FatherPassingYear; $scope.PASR_MotherPassingYear = promise.studentReg_DTObj[0].pasR_MotherPassingYear == 0 ? "" : promise.studentReg_DTObj[0].pasR_MotherPassingYear;




                $scope.PASR_TotalIncome = $scope.PASR_FatherIncome + $scope.PASR_MotherIncome;
                $scope.PASR_BirthPlace = promise.studentReg_DTObj[0].pasR_BirthPlace == null ? "" : promise.studentReg_DTObj[0].pasR_BirthPlace;
                $scope.studentnationality = promise.studentnationalitys.length > 0 ? promise.studentnationalitys[0].studentnationality : "";

                $scope.PASR_HostelReqdFlag = promise.studentReg_DTObj[0].pasR_HostelReqdFlag == 1 ? "Yes" : "No";//true : false
                $scope.PASR_TransportReqdFlag = promise.studentReg_DTObj[0].pasR_TransportReqdFlag == 1 ? "Yes" : "No";//true : false
                $scope.PASR_GymReqdFlag = promise.studentReg_DTObj[0].pasR_GymReqdFlag == 1 ? "Yes" : "No";//true : false
                $scope.PASR_ECSFlag = promise.studentReg_DTObj[0].pasR_ECSFlag == 1 ? "Yes" : "No";
                $scope.PASR_PaymentFlag = promise.studentReg_DTObj[0].pasR_PaymentFlag == 1 ? "Yes" : "No";

                $scope.PASR_AmountPaid = promise.studentReg_DTObj[0].pasR_AmountPaid;
                $scope.PASR_PaymentType = promise.studentReg_DTObj[0].pasR_PaymentType == null ? "" : promise.studentReg_DTObj[0].pasR_PaymentType;
                $scope.PASR_PaymentDate = promise.studentReg_DTObj[0].pasR_PaymentDate == null ? "" : promise.studentReg_DTObj[0].pasR_PaymentDate;
                $scope.PASR_ReceiptNo = promise.studentReg_DTObj[0].pasR_ReceiptNo == null ? "" : promise.studentReg_DTObj[0].pasR_ReceiptNo;
                //Activeflag
                //Applstatus
                $scope.PASR_FinalpaymentFlag = promise.studentReg_DTObj[0].pasR_FinalpaymentFlag == 1 ? "Yes" : "No";
                $scope.PASR_LastPlayGrndAttnd = promise.studentReg_DTObj[0].pasR_LastPlayGrndAttnd == null ? "" : promise.studentReg_DTObj[0].pasR_LastPlayGrndAttnd;
                $scope.PASR_ExtraActivity = promise.studentReg_DTObj[0].pasR_ExtraActivity == null ? "" : promise.studentReg_DTObj[0].pasR_ExtraActivity;
                $scope.PASR_OtherResidential_Addr = promise.studentReg_DTObj[0].pasR_OtherResidential_Addr == null ? "" : promise.studentReg_DTObj[0].pasR_OtherPermanentAddr;
                $scope.PASR_OtherPermanentAddr = promise.studentReg_DTObj[0].pasR_OtherPermanentAddr == null ? "" : promise.studentReg_DTObj[0].pasR_OtherPermanentAddr;
                $scope.PASR_FatherOfficeAddr = promise.studentReg_DTObj[0].pasR_FatherOfficeAddr == null ? "" : promise.studentReg_DTObj[0].pasR_FatherOfficeAddr;
                $scope.PASR_MotherOfficeAddr = promise.studentReg_DTObj[0].pasR_MotherOfficeAddr == null ? "" : promise.studentReg_DTObj[0].pasR_MotherOfficeAddr;
                $scope.PASR_UndertakingFlag = promise.studentReg_DTObj[0].pasR_UndertakingFlag == 1 ? "Yes" : "No";
                $scope.fathernationality = promise.fathernationalitys.length > 0 ? promise.fathernationalitys[0].fathernationality : "";
                $scope.mothernationality = promise.mothernationalitys.length > 0 ? promise.mothernationalitys[0].mothernationality : "";
                $scope.PASR_BirthCertificateNo = promise.studentReg_DTObj[0].pasR_BirthCertificateNo == null ? "" : promise.studentReg_DTObj[0].pasR_BirthCertificateNo;
                $scope.PASR_AltContactNo = promise.studentReg_DTObj[0].pasR_AltContactNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_AltContactNo;
                $scope.PASR_AltContactEmail = promise.studentReg_DTObj[0].pasR_AltContactEmail == null ? "" : promise.studentReg_DTObj[0].pasR_AltContactEmail;

                //PASR_Adm_Confirm_Flag
                //PAMS_Id
                //Id
                $('#blahnew').attr('src', promise.studentReg_DTObj[0].pasR_Student_Pic_Path);
                $('#blahfnew').attr('src', promise.studentReg_DTObj[0].pasR_FatherPhoto);
                $('#blahmnew').attr('src', promise.studentReg_DTObj[0].pasR_MotherPhoto);
                $('#blahnewa').attr('src', promise.studentReg_DTObj[0].pasR_Student_Pic_Path);

                $scope.studentphoto = promise.studentReg_DTObj[0].pasR_Student_Pic_Path;
                //PASL_ID
                //PASL_ID
                //Remark
                //Repeat_Class_Id
                // $scope.FMCC_Id = promise.studentReg_DTObj[0].fmcC_ID;
                $scope.PASR_Noofbrothers = promise.studentReg_DTObj[0].pasR_Noofbrothers == null ? "" : promise.studentReg_DTObj[0].pasR_Noofbrothers;
                $scope.PASR_Noofsisters = promise.studentReg_DTObj[0].pasR_Noofsisters == null ? "" : promise.studentReg_DTObj[0].pasR_Noofsisters;
                $scope.PASR_lastclassperc = promise.studentReg_DTObj[0].pasR_lastclassperc == null ? "" : promise.studentReg_DTObj[0].pasR_lastclassperc;
                $scope.PASR_SibblingConcessionFlag = promise.studentReg_DTObj[0].pasR_SibblingConcessionFlag == 1 ? "Yes" : "No";
                $scope.PASR_ParentConcessionFlag = promise.studentReg_DTObj[0].pasR_ParentConcessionFlag == 1 ? "Yes" : "No";

                //// guardian
                if (promise.studentGuardian_DTObj != null && promise.studentGuardian_DTObj != "") {
                    $scope.pasrG_Id = promise.studentGuardian_DTObj[0].pasrG_Id;
                    $scope.PASRG_GuardianName = promise.studentGuardian_DTObj[0].pasrG_GuardianName;
                    $scope.PASRG_GuardianAddress = promise.studentGuardian_DTObj[0].pasrG_GuardianAddress;
                    $scope.PASRG_GuardianRelation = promise.studentGuardian_DTObj[0].pasrG_GuardianRelation;
                    $scope.PASRG_emailid = promise.studentGuardian_DTObj[0].pasrG_emailid;
                    $scope.PASRG_GuardianPhoneNo = promise.studentGuardian_DTObj[0].pasrG_GuardianPhoneNo;
                    $scope.PASRG_Occupation = promise.studentGuardian_DTObj[0].pasrG_Occupation;
                    $scope.PASRG_PhoneOffice = promise.studentGuardian_DTObj[0].pasrG_PhoneOffice;
                }
                ////

                //// Sibling
                if (promise.studentSbling_DTObj.length > 0) {
                    $scope.siblingsprint = promise.studentSbling_DTObj;

                    if ($scope.siblingsprint.length == 1) {
                        $scope.firstsibling = $scope.siblingsprint[0].pasrS_SiblingsName;
                        $scope.firstsiblingclass = $scope.siblingsprint[0].pasrS_SiblingsClass;
                    }
                    if ($scope.siblingsprint.length == 2) {

                        $scope.firstsibling = $scope.siblingsprint[0].pasrS_SiblingsName;
                        $scope.firstsiblingclass = $scope.siblingsprint[0].pasrS_SiblingsClass;

                        $scope.secondsibling = $scope.siblingsprint[1].pasrS_SiblingsName;
                        $scope.secondsiblingclass = $scope.siblingsprint[1].pasrS_SiblingsClass;
                    }



                    $scope.showbr = false;
                    $scope.siblingshow = true;
                }
                else {
                    $scope.sibl = "No!";
                    $scope.showbr = true;
                    $scope.siblingshow = false;
                }
                if (promise.academicdrp.length > 0) {
                    $scope.ASMAY_Year = promise.academicdrp[0].asmaY_Year;
                }

                if (promise.studenthelthDTO != null) {
                    if (promise.studenthelthDTO.length > 0) {
                        $scope.albumNameArray1 = [];
                        $scope.albumNameArray2 = [];
                        for (var i = 0; i < promise.studenthelthDTO.length; i++) {
                            if (promise.studenthelthDTO[i].pasR_FirstName != '') {
                                if (promise.studenthelthDTO[i].pasR_MiddleName !== null) {
                                    if (promise.studenthelthDTO[i].pasR_LastName !== null) {

                                        $scope.albumNameArray1.push({ name: promise.studenthelthDTO[i].pasR_FirstName + " " + promise.studenthelthDTO[i].pasR_MiddleName + " " + promise.studenthelthDTO[i].pasR_LastName });
                                    }
                                    else {
                                        $scope.albumNameArray1.push({ name: promise.studenthelthDTO[i].pasR_FirstName + " " + promise.studenthelthDTO[i].pasR_MiddleName });
                                    }
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: promise.studenthelthDTO[i].pasR_FirstName });
                                }
                            }
                        }

                        $scope.Hstuname = $scope.albumNameArray1[0].name;
                        $scope.Hstuage = promise.studenthelthDTO[0].pasR_Age;
                        $scope.HFirstName = promise.studenthelthDTO[0].pasR_FatherName;
                        $scope.VaccinationDate = promise.studenthelthDTO[0].pashD_VaccinationDate;
                        if (promise.studenthelthDTO[0].pashD_FitsFlag == 1) {
                            $scope.FitsFlag = "Yes";
                            $scope.FitsDate = promise.studenthelthDTO[0].pashD_FitsDate;
                        }
                        else {
                            $scope.FitsFlag = "No";
                            $scope.FitsDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_ChickenpoxFlag == 1) {
                            $scope.chickenFlag = "Yes";
                            $scope.cihickenDate = promise.studenthelthDTO[0].pashD_ChickenpoxDate;
                        }
                        else {
                            $scope.chickenFlag = "No";
                            $scope.cihickenDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_DiptheriaFlag == 1) {
                            $scope.dipFlag = "Yes";
                            $scope.dipDate = promise.studenthelthDTO[0].pashD_DiptheriaDate;
                        }
                        else {
                            $scope.dipFlag = "No";
                            $scope.dipDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_EpidemicFlag == 1) {
                            $scope.epideFlag = "Yes";
                            $scope.epideDate = promise.studenthelthDTO[0].pashD_EpidemicDate;
                        }
                        else {
                            $scope.epideFlag = "No";
                            $scope.epideDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_MeaslesFlag == 1) {
                            $scope.measleFlag = "Yes";
                            $scope.measleDate = promise.studenthelthDTO[0].pashD_MeaslesDate;
                        }
                        else {
                            $scope.measleFlag = "No";
                            $scope.measleDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_MumpsFlag == 1) {
                            $scope.mumFlag = "Yes";
                            $scope.mumDate = promise.studenthelthDTO[0].pashD_MumpsDate;
                        }
                        else {
                            $scope.mumFlag = "No";
                            $scope.mumDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_RingwormFlag == 1) {
                            $scope.ringFlag = "Yes";
                            $scope.ringDate = promise.studenthelthDTO[0].pashD_RingwormDate;
                        }
                        else {
                            $scope.ringFlag = "No";
                            $scope.ringDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_ScarletFlag == 1) {
                            $scope.scarletFlag = "Yes";
                            $scope.scarletDate = promise.studenthelthDTO[0].pashD_ScarletDate;
                        }
                        else {
                            $scope.scarletFlag = "No";
                            $scope.scarletDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_SmallPoxFlag == 1) {
                            $scope.smallFlag = "Yes";
                            $scope.smallDate = promise.studenthelthDTO[0].pashD_SmallPoxDate;
                        }
                        else {
                            $scope.smallFlag = "No";
                            $scope.smallDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_WhoopingFlag == 1) {
                            $scope.whoFlag = "Yes";
                            $scope.whoDate = promise.studenthelthDTO[0].pashD_WhoopingDate;
                        }
                        else {
                            $scope.whoFlag = "No";
                            $scope.whoDate = " ";
                        }

                        if (promise.studenthelthDTO[0].pashD_Illness == 1) {
                            $scope.Illness = "Yes";

                        }
                        else {
                            $scope.Illness = "No";

                        }

                        //$scope.Illness = promise.studenthelthDTO[0].pashD_Illness;
                        $scope.HepatitisB = new Date(promise.studenthelthDTO[0].pashD_HepatitisB);
                        $scope.TyphoidFever = new Date(promise.studenthelthDTO[0].pashD_TyphoidFever);
                        $scope.Ailment = promise.studenthelthDTO[0].pashD_Ailment;
                        if (promise.studenthelthDTO[0].pashD_Allergy == 1) {
                            $scope.Allergy = "Yes";
                        }
                        else {
                            $scope.Allergy = "No";
                        }

                        $scope.HealthProblem = promise.studenthelthDTO[0].pashD_HealthProblem;
                        $scope.BloodGroup = promise.studenthelthDTO[0].pashD_BloodGroup;
                    }
                }

                if (promise.studentPrevSch_DTObj.length > 0) {
                    $scope.PreviousSchoolList = promise.studentPrevSch_DTObj;

                    $scope.schoolname = $scope.PreviousSchoolList[0].pasrpS_PrvSchoolName
                    $scope.schooladress = $scope.PreviousSchoolList[0].pasrpS_Address
                    $scope.lastclass = $scope.PreviousSchoolList[0].pasrpS_PreviousClass
                    $scope.lastsylaabus = $scope.PreviousSchoolList[0].pasrpS_Board
                }
                $('#blahfnew').attr('src', promise.studentReg_DTObj[0].pasR_FatherPhoto);
                $('#blahmnew').attr('src', promise.studentReg_DTObj[0].pasR_MotherPhoto);
                $('#blahnewa').attr('src', promise.studentReg_DTObj[0].pasR_Student_Pic_Path);
                var e1 = angular.element(document.getElementById("test"));
                $compile(e1.html(promise.htmldata))(($scope));

                $('#blahnew').attr('src', $scope.studentphoto);
                $('#blahnewa').attr('src', promise.studentReg_DTObj[0].pasR_Student_Pic_Path);
                $('#blahfnew').attr('src', promise.studentReg_DTObj[0].pasR_FatherPhoto);
                $('#blahmnew').attr('src', promise.studentReg_DTObj[0].pasR_MotherPhoto);

            });
        }

        $scope.BBSAPP = function () {

            var innerContents = document.getElementById("BBSAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/Hutchingspdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


        $scope.BGHSAPP = function () {

            var innerContents = document.getElementById("BGHSAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BGHS/BGHSAPPPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BCEHS/BCOESAPP.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BGHS/BGHSAPP.css"/>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.HHSAPP = function () {

            var innerContents = document.getElementById("HHSAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/Hutchingspdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.BBHSAPP = function () {

            var innerContents = document.getElementById("BBHSAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/Hutchingspdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.BCEHSAPP = function () {

            var innerContents = document.getElementById("BCOESAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BCEHS/BCOESAPP.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BCEHS/BCOESAPPReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.searchfilter = function (objj) {

            var data = {
                "searchfilter": objj.search,
                "filterinitialdata": $scope.filterdata,

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            /// $scope.studentlst = promise.fillstudent;
            apiService.create("CollegeFeePreadmissionTransaction/searchfilter", data).
                then(function (promise) {
                    $scope.studentlst = promise.fillstudent;
                    angular.forEach($scope.studentlst, function (objectt) {
                        if (objectt.pasR_FirstName.length > 0) {
                            var string = objectt.pasR_FirstName;
                            objectt.pasR_FirstName = string.replace(/  +/g, ' ');
                        }
                    })

                })
        }

        $scope.radioreload = function (filterdata) {
            if (filterdata == 'Preadmission') {
                $scope.nameonradio = "Student Name";
            }
            else {
                $scope.nameonradio = "Prospectus No.";
            }
            $scope.radshow = false;
            var data = {
                "filterinitialdata": $scope.filterdata,
            }

            apiService.create("CollegeFeePreadmissionTransaction/filterstudent", data).
                then(function (promise) {

                    if (promise.receiparraydelete.length > 0) {
                        $scope.saved_staff_list = promise.receiparraydelete;
                    }
                    else {
                        $scope.saved_staff_list.length = 0;
                    }

                })
        }

        $scope.printData = function (printmodal) {
            var innerContents = document.getElementById("printmodal").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link href="plugins/bootstrap/css/bootstrap.css" />' +
                '<link href="css/style.css" rel="stylesheet" />' +
                '</head><body onload="window.print()" onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
    }
})();