
(function () {
    'use strict';
    angular
.module('app')
        .controller('ClgDatewiseHeadCollectionController', ClgDatewiseHeadCollectionController)

    ClgDatewiseHeadCollectionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', 'Excel', '$timeout']
    function ClgDatewiseHeadCollectionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, Excel, $timeout) {
        $scope.show_btn = false;
        $scope.show_cancel = false;
        $scope.show_grid = false;
        $scope.searc_button = true;

        // $scope.sortReverse = true;
        $scope.betweendates = false;
        $scope.monthly = false;

        $scope.onclickloaddataclass = function () {
            if ($scope.rpttyp == "Individual") {
                $scope.betweendates = false;
                $scope.monthly = true;
            }
            else if ($scope.rpttyp == "All") {
                $scope.betweendates = true;
                $scope.monthly = false;
            }
        };


        var paginationformasters;
        $scope.page1 = "page1";
        $scope.page2 = "page2";
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else if (ivrmcofigsettings.length == 0 || ivrmcofigsettings == undefined || ivrmcofigsettings == null) {
            paginationformasters = 10;
        }

        paginationformasters = 10;

        $scope.currentPage = 1;
        $scope.currentPage2 = 1;

        $scope.itemsPerPage = paginationformasters;

        //=========For filter char count for first table===============//
        $scope.searchValue = '';
        $scope.search_box = function () {
            if ($scope.searchValue != "" || $scope.searchValue != null) {
                $scope.searc_button = false;
            }
            else {
                $scope.searc_button = true;
            }
        }
        //====================End================================//

        //=========For filter char count for Second table===============//
        $scope.searchValue1 = "";
        $scope.search_box1 = function () {
            if ($scope.searchValue1 != "" || $scope.searchValue1 != null) {
                $scope.searc_button1 = false;
            }
            else {
                $scope.searc_button1 = true;
            }
        }
        //====================End================================//
            //============Start checkbox selection Page==============//

        $scope.printdatatablegrp = [];
        $scope.totalamt = 0;
        $scope.optionToggledgrp = function (SelectedStudentRecord, index) {

            $scope.totalamt = 0;
           
            debugger;
            $scope.grpall = $scope.feedetails.every(function (itm) { return itm.grpselected; });

            if ($scope.printdatatablegrp.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatablegrp.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatablegrp.splice($scope.printdatatablegrp.indexOf(SelectedStudentRecord), 1);
            }


            if ($scope.printdatatablegrp.length > 0) {
                angular.forEach($scope.printdatatablegrp, function (ff) {
                    $scope.totalamt = $scope.totalamt + ff.PaidAmount;
                })

                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }

        }

        $scope.toggleAllgrp = function () {
            $scope.totalamt = 0;
            // $scope.printdatatablegrp = [];
            var toggleStatus = $scope.grpall;
            angular.forEach($scope.feedetails, function (itm) {
                itm.grpselected = toggleStatus;
                if ($scope.grpall == true) {
                    $scope.printdatatablegrp.push(itm);
                }
                //else {
                //    $scope.printdatatablegrp.splice(itm);
                //}
            });
            $scope.totalamt = 0;
            angular.forEach($scope.printdatatablegrp, function (ff) {
                $scope.totalamt = $scope.totalamt + ff.PaidAmount;
            })
            if ($scope.printdatatablegrp.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }

        }
          //====================End================================//

        // $scope.sortKey = "acysT_RollNo";    //set the sortKey to the param passed
        // $scope.reverse = true;      //if true make it false and vice versa
        $scope.search = "";
        $scope.show_flag = false;


        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.sortReverse = !$scope.sortReverse; //if true make it false and vice versa
        }
        $scope.studentlist = [];


        //============Start Data Load on the Page==============//
        $scope.loaddata = function () {

            
            var pageid = 1;
            apiService.getURI("ClgDatewiseHeadCollection/GetYearList", pageid).
                then(function (promise) {
                    
                    $scope.yearlist = promise.yearlist;
                    $scope.Monthlist = promise.monthlist;
                    // $scope.show_flag = false;
                })
        }
        //====================End===================//
        //===========Group all check and sinfle check===============//
        $scope.grpallcheck = function () {
            var toggleStatus = $scope.checkallgrp;
            angular.forEach($scope.fillmastergroup, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_heads();
        }

        $scope.grpsinglecheck = function () {
          
            $scope.checkallgrp = $scope.fillmastergroup.every(function (itm) {

                return itm.selected;
            });

            $scope.get_heads();
        };
        $scope.hrdallcheck = function () {
            var toggleStatus1 = $scope.checkallhrd;
            angular.forEach($scope.fillmasterhead, function (itm) {
                itm.selected = toggleStatus1;
            });
           
        }

        $scope.hrgsinglecheck = function () {

            $scope.checkallhrd = $scope.fillmasterhead.every(function (itm) {

                return itm.selected;
            });

           
        };




         //====================End===================//
        //===========Load ALL group data in to the CheckboxList===============//
        $scope.fillmastergroup = [];
        $scope.get_feegroups = function () {
            
            $scope.fillmastergroup = [];
            $scope.show_flag = false;
            if ($scope.asmaY_Id != undefined && $scope.asmaY_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id
                }
                apiService.create("ClgDatewiseHeadCollection/get_feegroups", data).then(function (promise) {
                    $scope.fillmastergroup = promise.fillmastergroup;
                    // $scope.amcO_Id = "";
                    if ($scope.fillmastergroup.length == 0 || $scope.fillmastergroup == null) {
                        swal('For Selected Year Courses Are Not Available!!!');

                    }
                })
            }
            else {
                $scope.courselist = [];
                $scope.amcO_Id = "";
            }
            $scope.show_btn = false;
            $scope.show_cancel = false;

            $scope.show_grid = false;
        };
        //====================End===================//


        //================load the fee heads CheckboxList=====================//
        $scope.get_heads = function () {
            $scope.fillmasterhead = [];
            var idc = [];
            angular.forEach($scope.fillmastergroup, function (crs) {
                if (crs.selected) {
                    $scope.fmG_Id = crs.fmG_Id;
                    idc.push(crs.fmG_Id);
                }
            })
            if (idc.length > 0) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    FMG_Ids: idc
                }
                apiService.create("ClgDatewiseHeadCollection/get_heads", data).then(function (promise) {
                    $scope.fillmasterhead = promise.fillmasterhead;
                    // $scope.amB_Id = "";
                    console.log($scope.branchlist)
                    if ($scope.fillmasterhead.length == 0 || $scope.fillmasterhead == null) {
                        swal('For Selected Groups, Heada Are Not Available!!!');

                    }
                })
               
            }
            else {
                $scope.isOptionsRequired();
            }
          
        }
        //========================END==================================//


   
        //============================Save For GroupList Data=========================//
        $scope.valsgroup = [];
        $scope.valshead = [];
        $scope.valsinstallment = [];
        $scope.valstudentlst = [];
        //$scope.studentdata = promise.studentlist
        $scope.show_grid = false;
        $scope.totalamt1 = 0;
        $scope.savedata = function (grouplst, headlst) {
            $scope.totalamt1 = 0;
            $scope.feedetails = [];
            $scope.valsgroup = [];
            $scope.valshead = [];
            $scope.valsinstallment = [];
            $scope.valstudentlst = [];
            $scope.page1 = "page1";
            $scope.show_grid = false;
          

            if ($scope.myForm.$valid) {

                for (var t = 0; t < grouplst.length; t++) {
                    if (grouplst[t].selected == true) {
                        $scope.valsgroup.push(grouplst[t]);
                    }
                }

                for (var u = 0; u < headlst.length; u++) {
                    if (headlst[u].selected == true) {
                        $scope.valshead.push(headlst[u]);
                    }
                }

                grouplst = $scope.valsgroup;
                headlst = $scope.valshead;

                if ($scope.betweendates == true) {
                    var fromdate1 = new Date($scope.fromdate).toDateString();
                    var todate1 = new Date($scope.todate).toDateString();
                     $scope.monthid = 0;
                    var typ = "betweendates";
                }
                else if ($scope.betweendates == false) {
                    var fromdate1 = new Date(new Date()).toDateString();
                    var todate1 = new Date(new Date()).toDateString();
                    $scope.monthid = $scope.obj.amM_ID;
                    var typ = "monthly";
                }

                //if ($scope.rpttyp == "Individual") {

                //    var date = new Date();
                //    var fromdate1 = new Date(date.getFullYear(), date.getMonth(), 1);
                //    var todate1 = new Date(date.getFullYear(), date.getMonth() + 1, 0);

                //    fromdate1 = $scope.fromdate == null ? "" : $filter('date')(fromdate1, "yyyy-MM-dd");
                //    todate1 = $scope.todate == null ? "" : $filter('date')(todate1, "yyyy-MM-dd");
                //}
                //else {
                //    var fromdate1 = $scope.fromdate == null ? "" : $filter('date')($scope.fromdate, "yyyy-MM-dd");
                //    var todate1 = $scope.todate == null ? "" : $filter('date')($scope.todate, "yyyy-MM-dd");
                //}

               

                    if ($scope.valsgroup.length > 0)
                    {
                        var data = {
                            "ASMAY_Id": $scope.asmaY_Id,
                            savegrplst: grouplst,
                            saveheadlst: headlst,
                            "fromdate": fromdate1,
                            "todate": todate1,
                            "monthid": $scope.monthid,
                            "Typ": typ
                        }

                        var config = {
                            headers: {
                                'Content-Type': 'application/json;'
                            }
                        }

                        apiService.create("ClgDatewiseHeadCollection/savedata", data).
                        then(function (promise) {

                            $scope.feedetails = promise.feedetails;

                            if ($scope.feedetails.length > 0) {
                                $scope.show_grid = true;
                                angular.forEach($scope.feedetails, function (zz) {
                                    $scope.totalamt1 += zz.PaidAmount;
                                })

                            } else {
                                swal("No Record Found!!!");  
                            }

                        })
                    }
                    else {
                        swal("Select Atleast One Group!!!");
                    }
              
                
            }
            else {
                $scope.submitted = true;
            }
        };
        //======================End=======================//



        //===================Cancel========================//
        $scope.cancel = function () {
            $state.reload();
        }
        //===================End========================//


        //===========Field Validation=================//
        $scope.interacted = function (field) {

            return $scope.submitted;
        };
        //==================End===========================//


        //========Branchlist CheckBox Field Validation===========//
        $scope.isOptionsRequired_1 = function () {
            return !$scope.fillmasterhead.some(function (options) {
                return options.selected;
            });
        }
        //==================End===========================//


        //========courselist CheckBox Field Validation===========//
        $scope.isOptionsRequired = function () {
            return !$scope.fillmastergroup.some(function (options) {
                return options.selected;
            });
        }
        //==================End===========================//


        //========semesterlist CheckBox Field Validation============//
        //$scope.isOptionsRequired_2 = function () {
        //    return !$scope.semesterlist.some(function (options) {
        //        return options.selected1;

        //    });
        //}
        //==================End===========================//
        $scope.exportToExcel = function () {


            var exportHref = Excel.tableToExcel(tablegrp, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }
      
        $scope.printData = function () {
            if ($scope.printdatatablegrp !== null && $scope.printdatatablegrp.length > 0) {
                var innerContents = document.getElementById("printgrdgrp").innerHTML;
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


        }

        $scope.isOptionsRequired1 = function () {
            if ($scope.AMCST_idedit > 0) {
                return false;
            }
            else {
                return !$scope.grouplst.some(function (options) {
                    return options.checkedgrplst;
                });
            }
            
        }
        $scope.isOptionsRequirededit1 = function () {
            if ($scope.AMCST_idedit > 0)
            {
            return !$scope.grouplstedit.some(function (options) {
                    return options.checkedgrplstedit;
                });
            }
            else {
                return false;
            }
        }
    }
})();

