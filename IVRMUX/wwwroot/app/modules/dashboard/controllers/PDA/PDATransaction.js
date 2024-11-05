(function () {
    'use strict';
    angular
        .module('app')
        .controller('PDATransactionController', PDATransactionController)
    PDATransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', 'superCache']
    function PDATransactionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, superCache) {


        $scope.studentsavedlist = true;

        $scope.printreceipt = false;

        $scope.printview = true;


        $scope.obj = {};
        //added on 09102017
        $scope.allcheck = false;

        $scope.totcountsearch = 0;
        $scope.disablefine = true;
        $scope.disableconcession = true;
        $scope.disablenetamount = true;
        $scope.disablefsS_CurrentYrCharges = true;
        $scope.disablefsS_TotalToBePaid = true;
        $scope.showstudentname = true;
        $scope.rolenamelist = "";

        var institutionid, automanualreceiptnotranum

        var grouporterm, autoreceipt, receiptformat, mergeinstallment, fineapplicable;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        var transactionnumbering = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transactionnumbering.length; i++) {
            if (transactionnumbering.length > 0) {
                if (transactionnumbering[i].imN_Flag == "PDA") {
                    $scope.transnumbconfigurationsettingsassign = transactionnumbering[i];
                }
            }
        }
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        if (configsettings.length > 0) {
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            receiptformat = configsettings[0].fmC_Receipt_Format;
            mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
            fineapplicable = configsettings[0].fmC_FineEnableDisable;
        }

        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.reloaddata = function () {

            $scope.groupcount = [];
            $scope.grigview1 = false;

        }

        $scope.search = '';
        $scope.filterOnLocation = function (user1) {
            return angular.lowercase(user1.amsT_FirstName + ' ' + user1.amsT_MiddleName + ' ' + user1.amsT_LastName).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(user1.classname).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(user1.sectionname).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(user1.amsT_AdmNo).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(user1.fyP_Receipt_No).indexOf(angular.lowercase($scope.search)) >= 0 || JSON.stringify(user1.fyP_Tot_Amount).indexOf($scope.search) >= 0 || ($filter('date')(user1.fyP_Date, 'dd-MM-yyyy').indexOf($scope.search) >= 0);

        };

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


        $scope.loaddata = function () {
            $scope.FYP_Date = new Date();
            $scope.currentPage = 1;
            $scope.studentsavedlist = true;
            $scope.filterdata = "indivstd";
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            $scope.onclickloaddata();
            apiService.getURI("PDATransaction/getalldetails", pageid).
                then(function (promise) {

                    $scope.receiptgrid = promise.receiparraydelete;
                    $scope.rolenamelist = promise.rolename;
                    $scope.classcount = promise.classlist;
                    $scope.yearlst = promise.fillyear;
                    $scope.head = promise.pdadata;
                    $scope.headlist = promise.pdadata;
                    $scope.totalgrid = [];
                    if ($scope.totalgrid.length == 0) {
                        $scope.totalgrid.push({
                            pdamH_Id: '',
                            PDAEH_Amount: '',
                            PDAE_Remarks: '',
                        });

                    }
                    $scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;

                    if (promise.transnumconfig != null) {
                        if (promise.transnumconfig.length > 0) {

                            automanualreceiptnotranum = promise.transnumconfig[0].imN_AutoManualFlag;

                        }

                        if (autoreceipt == 1 || promise.transnumconfig[0].imN_AutoManualFlag == "Auto") {
                            $scope.showreceiptno = false;
                        }
                        else {
                            $scope.showreceiptno = true;
                        }
                    }
                    $scope.receiptgrid = promise.receiparraydelete;

                    // $scope.getdates(promise.asmaY_Id, promise.asmaY_Year);
                })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }



        $scope.is_optionrequired_std = function () {

            return !$scope.sectioncount.some(function (options) {
                return options.selected;
            });
        }


        $scope.interacted2 = function (field) {

            return $scope.submitted2;
        };

        $scope.submitted2 = false;

        $scope.totalgridtest = [];
        $scope.addNew = function (totalgrid) {
            if ($scope.myForm.dataForm.$valid) {
                $scope.totalgrid.push({
                    pdamH_Id: '',
                    PDAEH_Amount: '',
                    PDAE_Remarks: '',
                });
            }
            else {
                $scope.submitted2 = true;
            }


        };
        $scope.removerow = function (totalgrid) {
            $scope.totalgrid.splice($scope.totalgrid.length - 1, 1);

        };



        $scope.searchfilter = function (objj, radioobj) {

            if (institutionid == '5' || institutionid == '4' || institutionid == '3') {
                if (objj.search.length >= '3' && radioobj == 'regular') {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("PDATransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;

                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }

                if (objj.search.length >= '4' && radioobj == 'AdmNo') {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("PDATransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;

                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })

                        })
                }

                else if (objj.search.length >= '6' && radioobj == 'regno') {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("PDATransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;

                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })

                        })
                }

                else if (objj.search.length >= '3' && radioobj == 'NameAdmno') {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("PDATransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }


                else if (objj.search.length >= '4' && radioobj == 'Admnoname') {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("PDATransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }

                else if (objj.search.length >= '3' && radioobj == 'NameRegNo') {

                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("PDATransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }


                else if (objj.search.length >= '4' && radioobj == 'RegNoName') {

                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("PDATransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }

                else if (objj.search.length >= '3' && radioobj == 'left') {
                    // $scope.studentlst = {};
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    /// $scope.studentlst = promise.fillstudent;
                    apiService.create("FeeStudentTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }

                else if (objj.search.length >= '2' && radioobj == 'inactive') {
                    // $scope.studentlst = {};
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    /// $scope.studentlst = promise.fillstudent;
                    apiService.create("FeeStudentTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }








            }
            else {

                if (objj.search.length >= '1' && radioobj == 'regular') {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("PDATransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }

                if (objj.search.length >= '2' && radioobj == 'AdmNo') {

                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("PDATransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }

                else if (objj.search.length >= '2' && radioobj == 'regno') {

                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("PDATransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }

                else if (objj.search.length >= '2' && radioobj == 'NameAdmno') {

                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("PDATransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }


                else if (objj.search.length >= '2' && radioobj == 'Admnoname') {
                    // $scope.studentlst = "";
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("PDATransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }



                else if (objj.search.length >= '3' && radioobj == 'left') {
                    // $scope.studentlst = {};
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    /// $scope.studentlst = promise.fillstudent;
                    apiService.create("FeeStudentTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }

                else if (objj.search.length >= '2' && radioobj == 'inactive') {
                    // $scope.studentlst = {};
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    /// $scope.studentlst = promise.fillstudent;
                    apiService.create("FeeStudentTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }



            }

        };



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





        $scope.onclickloaddata = function () {

            if ($scope.filterdata == "indivstd") {

                $scope.student = true;
                $scope.multiple = false;

            }
            else if ($scope.filterdata == "multiplestd") {

                $scope.multiple = true;
                $scope.student = false;


            }

            $scope.submitted = false;
            // $scope.myForm.$setPristine();
            // $scope.myForm.$setUntouched();
        };



        $scope.onselectclass = function () {
            $scope.asmS_Id = '';
            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }


            apiService.create("PDATransaction/getsection", data).
                then(function (promise) {
                    $scope.sectioncount = promise.sectionlist;

                })
        }

        $scope.onselectsection = function () {
            var ASMCL_Ids = [];
            // var toggleStatus = $scope.grpall;
            //  if (toggleStatus == true) {
            //angular.forEach($scope.sectioncount, function (itm) {
            //    itm.selected = true;
            //});
            // }
            //else {
            //    angular.forEach($scope.studentlst, function (itm) {
            //        itm.selectedstd = false;
            //    });
            //}


            angular.forEach($scope.sectioncount, function (ty) {
                if (ty.sectionlst == true) {
                    ASMCL_Ids.push(ty.asmS_Id);
                }
            })

            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                ASMCL_Ids: ASMCL_Ids

                // "AMSC_Id": $scope.asmS_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }


            apiService.create("PDATransaction/getstudent", data).
                then(function (promise) {
                    $scope.studentlst = promise.admsudentslist;


                    $scope.totalgridlst = [];
                    if ($scope.totalgridlst.length == 0) {
                        angular.forEach($scope.studentlst, function (ty) {
                            $scope.totalgridlst.push({
                                pdamH_Id: '',
                                PDAEH_Amount: '',
                                PDAE_Remarks: '',
                                AMST_StudentName: ty.amsT_FirstName,
                                AMST_Id: ty.amst_Id,
                                asmcL_Classname: ty.asmcL_Classname,
                                asmc_sectionname: ty.asmc_sectionname,
                                amaY_RollNo: ty.trmR_Id
                            });
                        });

                    }

                })
        }



        $scope.ShowSearch_Report = function () {


            if ($scope.searchtxt != "" || $scope.searchnumbr != "" || $scope.searchdat != "") {
                if ($scope.search123 == "5") {
                    var data = {
                        "searchType": $scope.search123,
                        "searchnumber": $scope.searchnumbr,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    }
                }
                else if ($scope.search123 == "4") {

                    var date = new Date($scope.searchdat).toDateString("dd/MM/yyyy");
                    var data = {
                        "searchType": $scope.search123,
                        "searchdate": date,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    }
                }
                else {

                    var data = {
                        "searchType": $scope.search123,
                        "searchtext": $scope.searchtxt,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    }

                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("PDATransaction/searching", data).
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





        $scope.toggleAllgrp = function () {

            var AMST_Ids = [];
            var toggleStatus = $scope.grpall;
            if (toggleStatus == true) {
                angular.forEach($scope.studentlst, function (itm) {
                    itm.selectedstd = true;
                });
            }
            else {
                angular.forEach($scope.studentlst, function (itm) {
                    itm.selectedstd = false;
                });
            }

        }

        $scope.toggleAll = function () {

          
            var toggleStatus = $scope.all;
            if (toggleStatus == true) {
                angular.forEach($scope.totalgridlst, function (itm) {
                    itm.isSelected = true;
                });
            }
            else {
                angular.forEach($scope.totalgridlst, function (itm) {
                    itm.isSelected = false;
                });
            }

        }

        $scope.amtdetails = function () {
            
            $scope.all = $scope.totalgridlst.every(function (itm) {
                return itm.isSelected;
            });

        };


        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.submitted = false;
        $scope.savedata = function () {
            var AMST_Ids = [];
            var temparr = [];
            var headdetails = [];
            var data = {};
            if ($scope.myForm.$valid) {

                var totalamt = 0;
                angular.forEach($scope.totalgrid, function (value, key) {
                    totalamt += Number(value.PDAEH_Amount);


                    headdetails.push({ pdamH_Id: value.pdamH_Id.pdamH_Id, PDAEH_Amount: value.PDAEH_Amount, PDAE_Remarks: value.PDAE_Remarks });

                })


                $scope.totalfee = totalamt;



                if ($scope.filterdata != "multiplestd") {
                    data = {
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "PDAE_ID": $scope.PDAE_ID,
                        savetmpdata: headdetails,
                        "Amst_Id": $scope.Amst_Id.amst_Id,
                        "amount": $scope.totalfee,
                        // "transactionno": $scope.transactionno,
                        "PDAE_Date": new Date($scope.FYP_Date).toDateString(),
                        "AMST_Ids": AMST_Ids,
                        "type": $scope.filterdata,
                        "PDAR_ModeOfPayment": $scope.trntype,

                        "transnumconfigsettings": $scope.transnumbconfigurationsettingsassign
                    }
                }
                else {

                    angular.forEach($scope.totalgridlst, function (ty) {
                        if (ty.isSelected == true) {
                            temparr.push(ty)
                        }

                    })

                    data = {
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,

                        savetmpdata: temparr,

                        "amount": $scope.totalfee,
                        "transactionno": $scope.transactionno,
                        "PDAE_Date": new Date($scope.FYP_Date).toDateString(),
                       // AMST_Ids: AMST_Ids,
                        "type": $scope.filterdata,
                        "PDAR_ModeOfPayment": $scope.trntype,
                        "transnumconfigsettings": $scope.transnumbconfigurationsettingsassign
                    }
                }


                apiService.create("PDATransaction/Savedata", data).
                    then(function (promise) {


                        if (promise.returnval === true) {
                            swal('Record Save / Updated Successfully');


                        }
                        else {
                            if (promise.pdaR_BankName == 'Duplicate') {
                                swal('Receipt No Cannot Be Duplicate');
                            }
                            else {
                                swal('Record Not Saved');
                            }

                        }

                        $state.reload();
                    })


            }
            else {
                $scope.submitted2 = true;
                $scope.submitted = true;
            }

        };




        $scope.DeletRecord = function (employee, SweetAlert) {
            $scope.editEmployee = employee.pdaE_ID;
            var data = {
                "PDAE_ID": $scope.editEmployee,
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
                        apiService.create("PDATransaction/Deletedetails", data).
                            then(function (promise) {

                                $scope.thirdgrid = promise.alldata;


                                if (promise.returnval == true) {

                                    //$scope.masterse = promise.masterSectionData;

                                    swal('Record Deleted Successfully');
                                }
                                else {
                                    swal('Record Not Deleted Successfully');
                                }

                                $state.reload();
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });

        }


        $scope.expchange = function () {
            if ($scope.headlist != null && $scope.headlist.length > 0) {
                angular.forEach($scope.totalgridlst, function (objectt) {

                    objectt.pdamH_Id = $scope.pdamH_Idnew.pdamH_Id;
                    //$scope.head[0].pdamH_Id = objectt.pdamH_Id;
                    //$scope.head[0].Selected = true;
                })
            }


            //if()

            //if ($scope.exphead == true) {
            //    var pdamH_Id = $scope.pdamH_Id.pdamH_Id;






            //}
            //else {

            //}
        }

        $scope.amtchange = function () {

            if ($scope.comamt == true) {
                var amt = $scope.amt;

                angular.forEach($scope.totalgridlst, function (objectt) {
                    objectt.PDAEH_Amount = amt;

                })

            }
            else {

            }
        }

        $scope.remarkchange = function () {

            if ($scope.comremark == true) {
                var PDAE_Remark = $scope.PDAE_Remark;

                angular.forEach($scope.totalgridlst, function (objectt) {
                    objectt.PDAE_Remarks = PDAE_Remark;

                })

            }
            else {

            }
        }


        $scope.cleardata = function () {
            $state.reload();
        }

        $scope.clearsearch = function () {
            $state.reload();
        }
    }
})();



