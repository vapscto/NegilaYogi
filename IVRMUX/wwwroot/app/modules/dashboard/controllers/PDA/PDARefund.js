(function () {
    'use strict';
    angular
.module('app')
.controller('PDARefundController', PDARefundController)
    PDARefundController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', 'superCache']
    function PDARefundController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, superCache) {


        $scope.studentsavedlist = true;

        $scope.printreceipt = false;

        $scope.printview = true;


        $scope.obj = {};
        //added on 09102017
        $scope.allcheck = false;
        $scope.searchby = 0;
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
                if (transactionnumbering[i].imN_Flag == "Online") {
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

        if (configsettings[0].fmC_FeeSearchNoOfDigits != null) {
            $scope.searchby = configsettings[0].fmC_FeeSearchNoOfDigits;
        } else {
            $scope.searchby = 3;
        }

        $scope.reloaddata = function () {
            $scope.groupcount = [];
            $scope.grigview1 = false;
        }

        $scope.reloaddataleft = function () {          
            $scope.filterdata2 = false;
            $scope.filterdata1 = true;
            $scope.filterdata = "NameAdmno";
            //$scope.groupcount = [];
            //$scope.grigview1 = false;
        }

        $scope.reloaddatadeactive = function () {
           $scope.filterdata1 = false;
            $scope.filterdata2 = true;
            $scope.filterdata = "NameAdmno";
            //$scope.groupcount = [];
            //$scope.grigview1 = false;
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
            
            $scope.currentPage = 1;
            $scope.FYP_Date = new Date();
            $scope.studentsavedlist = true;
            $scope.rfdamt = false;
            $scope.FYP_Bank_Or_Cash = "C";
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            $scope.onselectmodeofpayment();
            apiService.getURI("PDARefund/getalldetails", pageid).
       then(function (promise) {
           
           //$scope.receiptgrid = promise.receiparraydelete;
           $scope.rolenamelist = promise.rolename;
           $scope.classcount = promise.classlist;
           //$scope.yearlst = academicyrlst;
           $scope.yearlst = promise.fillyear;
           $scope.head = promise.pdadata;
           $scope.totalgrid = [];
           if ($scope.totalgrid.length == 0) {
               $scope.totalgrid.push({
                   pdamH_Id: '',
                   PDAEH_Amount: '',
                   PDAE_Remarks: '',
               });

           }
           $scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;
           if (promise.transnumconfig.length > 0) {

               automanualreceiptnotranum = promise.transnumconfig[0].imN_AutoManualFlag;

           }

           if (autoreceipt == 1 || promise.transnumconfig[0].imN_AutoManualFlag == "Auto") {
               $scope.showreceiptno = false;
           }
           else {
               $scope.showreceiptno = true;
           }

           //$scope.receiptgrid = promise.receiparraydelete;

           $scope.getdates(promise.asmaY_Id, promise.asmaY_Year);
       })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }


        //OChange Of Academic Year
        $scope.onselectacademic = function (yearlst) {

            angular.forEach(yearlst, function (op_m) {
                if (op_m.asmaY_Id == $scope.cfg.ASMAY_Id) {
                    $scope.asmaY_Year = op_m.asmaY_Year
                }
            })

            var data = {
                "filterinitialdata": $scope.filterdata,
                "ASMAY_Id": $scope.cfg.ASMAY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("PDARefund/getacademicyear", data).
                then(function (promise) {

                    $scope.receiptgrid = promise.receiparraydelete;

                    $scope.amsT_FirstName = "";
                    $scope.amsT_MiddleName = "";
                    $scope.amsT_LastName = "";
                    $scope.amsT_AdmNo = "";
                    $scope.amsT_RegistrationNo = "";
                    $scope.amaY_RollNo = "";
                    $scope.amsT_mobile = "";
                    $scope.classname = "";
                    $scope.sectionname = "";
                    $scope.fathername = "";
                    $scope.studentdob = "";

                    $scope.getdates(promise.asmaY_Id, $scope.asmaY_Year);

                })
        };
        //Ended


        //Search Filter
        $scope.searchfilter = function (objj, radioobj) {

           if (objj.search.length >= $scope.searchby && radioobj == 'regular') {
                if ($scope.filterdata1 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "L"
                    }

                }
                else if ($scope.filterdata2 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "D"
                    }

                }                
                else {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "S"
                    }
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
               apiService.create("PDARefund/searchfilter", data).
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

            if (objj.search.length >= $scope.searchby && radioobj == 'AdmNo') {
                
                if ($scope.filterdata1 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "L"
                    }

                }
                else if ($scope.filterdata2 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "D"
                    }

                }              
                else {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "S"
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                /// $scope.studentlst = promise.fillstudent;
                apiService.create("PDARefund/searchfilter", data).
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

            else if (objj.search.length >= $scope.searchby && radioobj == 'regno') {
               
                if ($scope.filterdata1 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "L"
                    }

                }
                else if ($scope.filterdata2 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "D"
                    }

                }
                else {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "S"
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                /// $scope.studentlst = promise.fillstudent;
                apiService.create("PDARefund/searchfilter", data).
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

            else if (objj.search.length >= $scope.searchby && radioobj == 'NameAdmno') {
                
                if ($scope.filterdata1 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "L"
                    }

                }
                else if ($scope.filterdata2 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "D"
                    }

                }                           
                else {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "S"
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                /// $scope.studentlst = promise.fillstudent;
                apiService.create("PDARefund/searchfilter", data).
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


            else if (objj.search.length >= $scope.searchby && radioobj == 'Admnoname') {
               
                if ($scope.filterdata1 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "L"
                    }

                }
                else if ($scope.filterdata2 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "D"
                    }

                }              
                else {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "S"
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("PDARefund/searchfilter", data).
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

            else if (objj.search.length >= $scope.searchby && radioobj == 'NameRegNo') {

                if ($scope.filterdata1 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "L"
                    }

                }
                else if ($scope.filterdata2 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "D"
                    }

                }               
                else {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("PDARefund/searchfilter", data).
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


            else if (objj.search.length >= $scope.searchby && radioobj == 'RegNoName') {

               
                if ($scope.filterdata1 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "L"
                    }

                }
                else if ($scope.filterdata2 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "D"
                    }

                }                              
                else {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "S"
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("PDARefund/searchfilter", data).
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

            else if (objj.search.length >= $scope.searchby && radioobj == 'left') {
               
                if ($scope.filterdata1 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "L"
                    }

                }
                else if ($scope.filterdata2 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "D"
                    }

                }                              
                else {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "S"
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                /// $scope.studentlst = promise.fillstudent;
                apiService.create("PDARefund/searchfilter", data).
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


            else if (objj.search.length >= $scope.searchby && radioobj == 'inactive') {
             
                if ($scope.filterdata1 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "L"
                    }

                }
                else if ($scope.filterdata2 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "D"
                    }

                }                               
                else {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "S"
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                /// $scope.studentlst = promise.fillstudent;
                apiService.create("PDARefund/searchfilter", data).
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
        
        };
        //Ended


        //$scope.searchfilter = function (objj, radioobj) {

        //    if (institutionid == '5' || institutionid == '4' || institutionid == '3') {
        //        if (objj.search.length >= '3' && radioobj == 'regular') {
        //            var data = {
        //                "filterinitialdata": radioobj,
        //                "searchfilter": objj.search
        //            }
        //            var config = {
        //                headers: {
        //                    'Content-Type': 'application/json;'
        //                }
        //            }
        //            apiService.create("PDARefund/searchfilter", data).
        //                then(function (promise) {
        //                    $scope.studentlst = promise.fillstudent;

        //                    angular.forEach($scope.studentlst, function (objectt) {
        //                        if (objectt.amsT_FirstName.length > 0) {
        //                            var string = objectt.amsT_FirstName;
        //                            objectt.amsT_FirstName = string.replace(/  +/g, ' ');
        //                        }
        //                    })
        //                })
        //        }

        //        if (objj.search.length >= '4' && radioobj == 'AdmNo') {
        //            var data = {
        //                "filterinitialdata": radioobj,
        //                "searchfilter": objj.search
        //            }
        //            var config = {
        //                headers: {
        //                    'Content-Type': 'application/json;'
        //                }
        //            }

        //            apiService.create("PDARefund/searchfilter", data).
        //                then(function (promise) {
        //                    $scope.studentlst = promise.fillstudent;

        //                    angular.forEach($scope.studentlst, function (objectt) {
        //                        if (objectt.amsT_FirstName.length > 0) {
        //                            var string = objectt.amsT_FirstName;
        //                            objectt.amsT_FirstName = string.replace(/  +/g, ' ');
        //                        }
        //                    })

        //                })
        //        }

        //        else if (objj.search.length >= '6' && radioobj == 'regno') {
        //            var data = {
        //                "filterinitialdata": radioobj,
        //                "searchfilter": objj.search
        //            }
        //            var config = {
        //                headers: {
        //                    'Content-Type': 'application/json;'
        //                }
        //            }
        //            apiService.create("PDARefund/searchfilter", data).
        //                then(function (promise) {
        //                    $scope.studentlst = promise.fillstudent;

        //                    angular.forEach($scope.studentlst, function (objectt) {
        //                        if (objectt.amsT_FirstName.length > 0) {
        //                            var string = objectt.amsT_FirstName;
        //                            objectt.amsT_FirstName = string.replace(/  +/g, ' ');
        //                        }
        //                    })

        //                })
        //        }

        //        else if (objj.search.length >= '3' && radioobj == 'NameAdmno') {
        //            if ($scope.filterdata1 == true) {
        //                var data = {
        //                    "filterinitialdata": radioobj,
        //                    "searchfilter": objj.search,
        //                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
        //                    "AMST_SOL": "L"
        //                }
        //            }
        //            else if ($scope.filterdata2 == true) {
        //                var data = {
        //                    "filterinitialdata": radioobj,
        //                    "searchfilter": objj.search,
        //                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
        //                    "AMST_SOL": "D"
        //                }
        //            }
        //            else {
        //                var data = {
        //                    "filterinitialdata": radioobj,
        //                    "searchfilter": objj.search,
        //                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
        //                    "AMST_SOL": "S"
        //                }
        //            }
        //            var config = {
        //                headers: {
        //                    'Content-Type': 'application/json;'
        //                }
        //            }
        //            apiService.create("PDARefund/searchfilter", data).
        //                then(function (promise) {
        //                    $scope.studentlst = promise.fillstudent;
        //                    angular.forEach($scope.studentlst, function (objectt) {
        //                        if (objectt.amsT_FirstName.length > 0) {
        //                            var string = objectt.amsT_FirstName;
        //                            objectt.amsT_FirstName = string.replace(/  +/g, ' ');
        //                        }
        //                    })
        //                })
        //        }

        //        else if (objj.search.length >= '4' && radioobj == 'Admnoname') {
        //            var data = {
        //                "filterinitialdata": radioobj,
        //                "searchfilter": objj.search
        //            }
        //            var config = {
        //                headers: {
        //                    'Content-Type': 'application/json;'
        //                }
        //            }

        //            apiService.create("PDARefund/searchfilter", data).
        //                then(function (promise) {
        //                    $scope.studentlst = promise.fillstudent;
        //                    angular.forEach($scope.studentlst, function (objectt) {
        //                        if (objectt.amsT_FirstName.length > 0) {
        //                            var string = objectt.amsT_FirstName;
        //                            objectt.amsT_FirstName = string.replace(/  +/g, ' ');
        //                        }
        //                    })
        //                })
        //        }

        //        else if (objj.search.length >= '3' && radioobj == 'NameRegNo') {

        //            var data = {
        //                "filterinitialdata": radioobj,
        //                "searchfilter": objj.search
        //            }
        //            var config = {
        //                headers: {
        //                    'Content-Type': 'application/json;'
        //                }
        //            }

        //            apiService.create("PDARefund/searchfilter", data).
        //                then(function (promise) {
        //                    $scope.studentlst = promise.fillstudent;
        //                    angular.forEach($scope.studentlst, function (objectt) {
        //                        if (objectt.amsT_FirstName.length > 0) {
        //                            var string = objectt.amsT_FirstName;
        //                            objectt.amsT_FirstName = string.replace(/  +/g, ' ');
        //                        }
        //                    })
        //                })
        //        }

        //        else if (objj.search.length >= '4' && radioobj == 'RegNoName') {

        //            var data = {
        //                "filterinitialdata": radioobj,
        //                "searchfilter": objj.search
        //            }
        //            var config = {
        //                headers: {
        //                    'Content-Type': 'application/json;'
        //                }
        //            }

        //            apiService.create("PDARefund/searchfilter", data).
        //                then(function (promise) {
        //                    $scope.studentlst = promise.fillstudent;
        //                    angular.forEach($scope.studentlst, function (objectt) {
        //                        if (objectt.amsT_FirstName.length > 0) {
        //                            var string = objectt.amsT_FirstName;
        //                            objectt.amsT_FirstName = string.replace(/  +/g, ' ');
        //                        }
        //                    })
        //                })
        //        }

        //    }
        //    else {

        //        if (objj.search.length >= '1' && radioobj == 'regular') {
        //            var data = {
        //                "filterinitialdata": radioobj,
        //                "searchfilter": objj.search
        //            }
        //            var config = {
        //                headers: {
        //                    'Content-Type': 'application/json;'
        //                }
        //            }

        //            apiService.create("PDARefund/searchfilter", data).
        //                then(function (promise) {
        //                    $scope.studentlst = promise.fillstudent;
        //                    angular.forEach($scope.studentlst, function (objectt) {
        //                        if (objectt.amsT_FirstName.length > 0) {
        //                            var string = objectt.amsT_FirstName;
        //                            objectt.amsT_FirstName = string.replace(/  +/g, ' ');
        //                        }
        //                    })
        //                })
        //        }

        //        if (objj.search.length >= '2' && radioobj == 'AdmNo') {

        //            var data = {
        //                "filterinitialdata": radioobj,
        //                "searchfilter": objj.search
        //            }
        //            var config = {
        //                headers: {
        //                    'Content-Type': 'application/json;'
        //                }
        //            }

        //            apiService.create("PDARefund/searchfilter", data).
        //                then(function (promise) {
        //                    $scope.studentlst = promise.fillstudent;
        //                    angular.forEach($scope.studentlst, function (objectt) {
        //                        if (objectt.amsT_FirstName.length > 0) {
        //                            var string = objectt.amsT_FirstName;
        //                            objectt.amsT_FirstName = string.replace(/  +/g, ' ');
        //                        }
        //                    })
        //                })
        //        }

        //        else if (objj.search.length >= '2' && radioobj == 'regno') {

        //            var data = {
        //                "filterinitialdata": radioobj,
        //                "searchfilter": objj.search
        //            }
        //            var config = {
        //                headers: {
        //                    'Content-Type': 'application/json;'
        //                }
        //            }

        //            apiService.create("PDARefund/searchfilter", data).
        //                then(function (promise) {
        //                    $scope.studentlst = promise.fillstudent;
        //                    angular.forEach($scope.studentlst, function (objectt) {
        //                        if (objectt.amsT_FirstName.length > 0) {
        //                            var string = objectt.amsT_FirstName;
        //                            objectt.amsT_FirstName = string.replace(/  +/g, ' ');
        //                        }
        //                    })
        //                })
        //        }

        //        else if (objj.search.length >= '2' && radioobj == 'NameAdmno') {

        //            var data = {
        //                "filterinitialdata": radioobj,
        //                "searchfilter": objj.search
        //            }
        //            var config = {
        //                headers: {
        //                    'Content-Type': 'application/json;'
        //                }
        //            }

        //            apiService.create("PDARefund/searchfilter", data).
        //                then(function (promise) {
        //                    $scope.studentlst = promise.fillstudent;
        //                    angular.forEach($scope.studentlst, function (objectt) {
        //                        if (objectt.amsT_FirstName.length > 0) {
        //                            var string = objectt.amsT_FirstName;
        //                            objectt.amsT_FirstName = string.replace(/  +/g, ' ');
        //                        }
        //                    })
        //                })
        //        }

        //        else if (objj.search.length >= '2' && radioobj == 'Admnoname') {
        //            // $scope.studentlst = "";
        //            var data = {
        //                "filterinitialdata": radioobj,
        //                "searchfilter": objj.search
        //            }
        //            var config = {
        //                headers: {
        //                    'Content-Type': 'application/json;'
        //                }
        //            }

        //            apiService.create("PDARefund/searchfilter", data).
        //                then(function (promise) {
        //                    $scope.studentlst = promise.fillstudent;
        //                    angular.forEach($scope.studentlst, function (objectt) {
        //                        if (objectt.amsT_FirstName.length > 0) {
        //                            var string = objectt.amsT_FirstName;
        //                            objectt.amsT_FirstName = string.replace(/  +/g, ' ');
        //                        }
        //                    })
        //                })
        //        }
        //    }

        //};



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

                apiService.create("PDARefund/searching", data).
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





        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.submitted = false;
     

        $scope.DeletRecord = function (employee, SweetAlert) {
            $scope.editEmployee1 = employee.pdaR_Id;
            $scope.editEmployee2 = employee.amst_Id;
            $scope.editEmployee3 = employee.pdaE_TotAmount;
            var data = {
                "PDAR_Id": $scope.editEmployee1,
                "Amst_Id": $scope.editEmployee2,
                "PDAE_TotAmount": $scope.editEmployee3,
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
                   apiService.create("PDARefund/Deletedetails", data).
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





        $scope.onselectstudent = function (studentlst) {


            var studid = studentlst.amst_Id;

            //$scope.temptermarray = [];

            var data = {
                "Amst_Id": studid,
                "filterinitialdata": $scope.filterdata,
                //"configset": grouporterm,
                //"autoreceiptflag": autoreceipt
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("PDARefund/getstuddetails", data).
       then(function (promise) {

               $scope.totalamount = promise.studentdata;
               $scope.rfdamt = true;
       })
     };

        

        $scope.onselectmodeofpayment = function () {

            var data = {
                "modeofpayment": $scope.FYP_Bank_Or_Cash,
                "filterinitialdata": $scope.filterdata
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            if ($scope.FYP_Bank_Or_Cash == 'C') {

                $scope.bankdetails = false;
            }
            else if ($scope.FYP_Bank_Or_Cash == 'B' || $scope.FYP_Bank_Or_Cash == 'R' || $scope.FYP_Bank_Or_Cash == 'S') {

                $scope.bankdetails = true;
            }
            else {

                $scope.groupcount = [];
                $scope.cfg.ASMAY_Id = "";
                $scope.amst_Id = "";
                $scope.grigview1 = false;

            }

            if ($scope.FYP_Bank_Or_Cash == 'C') {
                $scope.bankdetails = false;
            }
            else if ($scope.FYP_Bank_Or_Cash == 'B') {
                $scope.bankdetails = true;
            }
        };

        $scope.submitted = false;

        $scope.savedata = function () {

            if ($scope.myForm.$valid) {

                if ($scope.FYP_Bank_Or_Cash="C") {
                    var data = {
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "Amst_Id": $scope.Amst_Id.amst_Id,
                        //  "totalamount": $scope.totalamount,
                        "refundamt": Number($scope.amtadjustment),
                        "transactionno": $scope.transactionno,
                        "PDARFD_Date": new Date($scope.FYP_Date).toDateString(),
                        "PDAR_ChequeDDDate": new Date(),
                        "PDAR_RefundRemarks": $scope.FYP_Remarks,
                        "PDAR_ModeOfPayment": $scope.FYP_Bank_Or_Cash,
                        "PDAMH_Id": $scope.pdamH_Id,
                    }
                }
                else {
                    var data = {
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "Amst_Id": $scope.Amst_Id.amst_Id,
                        //  "totalamount": $scope.totalamount,
                        "refundamt": Number($scope.amtadjustment),
                        "transactionno": $scope.transactionno,
                        "PDARFD_Date": new Date($scope.FYP_Date).toDateString(),
                        "PDAR_ChequeDDDate": $scope.FYP_DD_Cheque_Date,
                        "PDAR_ChequeDDNo": $scope.FYP_DD_Cheque_No,
                        "PDAR_BankName": $scope.FYP_Bank_Name,
                        "PDAR_RefundRemarks": $scope.FYP_Remarks,
                        "PDAR_ModeOfPayment": $scope.FYP_Bank_Or_Cash,
                        "PDAMH_Id": $scope.pdamH_Id,
                    }
                }

                apiService.create("PDARefund/Savedata", data).
                           then(function (promise) {
                               if (promise.returnval === true) {
                                   $scope.loaddata();
                                   if (promise.returnval === true) {
                                       if (promise.message != null) {
                                           swal('Record Updated Successfully', 'success');
                                           $state.reload();
                                       }
                                       else {
                                           swal('Record Saved Successfully', 'success');
                                           $state.reload();
                                       }
                                   }
                               }
                               else {
                                   if (promise.message != null) {
                                       swal('Record Not Updated', 'success');
                                   }
                                   else {
                                       swal('Record Not Saved', 'success');
                                   }
                               }
                           })

            }
            else {
                $scope.submitted = true;
            }

        };


        $scope.ShowSearch_Report = function () {
            

            if ($scope.searchtxt != "" || $scope.searchnumbr != "" || $scope.searchdat != "") {
                if ($scope.search123 == "5") {
                    var data = {
                        "searchType": $scope.search123,
                        "searchnumber": $scope.searchnumbr,
                    }
                }
                else if ($scope.search123 == "4") {
                    
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

                apiService.create("PDARefund/searching", data).
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





        $scope.cleardata = function () {
            $state.reload();
        }

        $scope.clearsearch = function () {
            $state.reload();
        }
    }
})();



