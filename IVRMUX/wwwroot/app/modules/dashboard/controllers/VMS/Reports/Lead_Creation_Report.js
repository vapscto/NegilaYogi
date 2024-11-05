(function () {
    'use strict';
    angular
        .module('app')
        .controller('Lead_Creation_ReportController', lead_Creation_ReportController);

    lead_Creation_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce', '$timeout','Excel'];
    function lead_Creation_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $timeout, Excel) {


        //=============image
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.maxDatemf = new Date();
        //loading start

        $scope.athflag = false;
        $scope.loadData = function () {
            var id = 2;

            apiService.getURI("SalesSMSEMAIL/getalldetails", id).
                then(function (promise) {

                    $scope.categorylist = promise.categorylist;
                    $scope.sourcelist = promise.sourcelist;
                    $scope.productlist = promise.productlist;
                    $scope.statuslist = promise.statuslist;
                    $scope.countrylist = promise.countrylist;
                    // $scope.All_Individual();
                    $scope.grid_view = false;
                });
        };

        $scope.searchValue = '';
        $scope.itemsPerPage = 15;
        $scope.currentPage = 1;

        ///CATEGORY SEARCH START
        $scope.searchchkbx = '';
        $scope.filterchkbxhous = function (obj) {
            return (angular.lowercase(obj.ismsmcA_CategoryName)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        $scope.togchkbxhouse = function () {
            $scope.usercheckhous = $scope.categorylist.every(function (options) {
                return options.housselect;
            });
        }
        $scope.all_checkhous = function () {
            var checkStatus = $scope.usercheckhous;
            angular.forEach($scope.categorylist, function (itm) {
                itm.housselect = checkStatus;
            });
        }

        ///CATEGORY SEARCH END 
        ///SOURCE SEARCH START
        $scope.searchchkbx1 = '';
        $scope.filterchkbxhous1 = function (obj) {
            return (angular.lowercase(obj.ismsmsO_SourceName)).indexOf(angular.lowercase($scope.searchchkbx1)) >= 0;
        }

        $scope.togchkbxhouse1 = function () {
            $scope.usercheckhous1 = $scope.sourcelist.every(function (options) {
                return options.select;
            });
        }
        $scope.all_checkhous1 = function () {
            var checkStatus = $scope.usercheckhous1;
            angular.forEach($scope.sourcelist, function (itm) {
                itm.select = checkStatus;
            });
        }

        ///SOURCE SEARCH END


        ///PRODUCT SEARCH START
        $scope.searchchkbx2 = '';
        $scope.filterchkbxhous2 = function (obj) {
            return (angular.lowercase(obj.ismsmpR_ProductName)).indexOf(angular.lowercase($scope.searchchkbx2)) >= 0;
        }

        $scope.togchkbxhouse2 = function () {
            $scope.usercheckhous2 = $scope.productlist.every(function (options) {
                return options.select;
            });
        }
        $scope.all_checkhous2 = function () {
            var checkStatus = $scope.usercheckhous2;
            angular.forEach($scope.productlist, function (itm) {
                itm.select = checkStatus;
            });
        }

        ///PRODUCT SEARCH END
        ///STATUS SEARCH START
        $scope.searchchkbx3 = '';
        $scope.filterchkbxhous3 = function (obj) {
            return (angular.lowercase(obj.ismsmsT_StatusName)).indexOf(angular.lowercase($scope.searchchkbx3)) >= 0;
        }

        $scope.togchkbxhouse3 = function () {
            $scope.usercheckhous3 = $scope.statuslist.every(function (options) {
                return options.select;
            });
        }
        $scope.all_checkhous3 = function () {
            var checkStatus = $scope.usercheckhous3;
            angular.forEach($scope.statuslist, function (itm) {
                itm.select = checkStatus;
            });
        }

        ///STATUS SEARCH END

        ///COUNTRY SEARCH START
        $scope.searchchkbx4 = '';
        $scope.filterchkbxhous4 = function (obj) {
            return (angular.lowercase(obj.ivrmmC_CountryName)).indexOf(angular.lowercase($scope.searchchkbx4)) >= 0;
        }

        $scope.togchkbxhouse4 = function () {
            $scope.usercheckhous5 = false;
            $scope.usercheckhous4 = $scope.countrylist.every(function (options) {
                return options.select;
            });
            $scope.get_state();
        }
        $scope.all_checkhous4 = function () {
            $scope.usercheckhous5 = false;
            var checkStatus = $scope.usercheckhous4;
            angular.forEach($scope.countrylist, function (itm) {
                itm.select = checkStatus;
            });

            $scope.get_state();
        }

        ///COUNTRY SEARCH END 
        ///COUNTRY SEARCH START
        $scope.searchchkbx5 = '';
        $scope.filterchkbxhous5 = function (obj) {
            return (angular.lowercase(obj.ivrmmS_Name)).indexOf(angular.lowercase($scope.searchchkbx5)) >= 0;
        }

        $scope.togchkbxhouse5 = function () {
            $scope.usercheckhous5 = $scope.statelist.every(function (options) {
                return options.select;
            });


        }
        $scope.all_checkhous5 = function () {
            var checkStatus = $scope.usercheckhous5;
            angular.forEach($scope.statelist, function (itm) {
                itm.select = checkStatus;
            });


        }

        ///COUNTRY SEARCH END



        //validation start
        $scope.interacted = function (field) {

            return $scope.submitted;
        };
        $scope.isOptionsRequired = function () {
            return !$scope.staff_types.some(function (options) {
                return options.selected;
            });
        };
        $scope.isOptionsRequired1 = function () {
            return !$scope.Department_types.some(function (options) {
                return options.selected;
            });
        };
        $scope.isOptionsRequired2 = function () {
            return !$scope.Designation_types.some(function (options) {
                return options.selected;
            });
        };
        $scope.sort = function (keyname) {

            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        //validation end
        //all-individual start




        //all-check button end
        // fill dep start
        $scope.get_state = function () {
        
            var groupidss = [];
            for (var i = 0; i < $scope.countrylist.length; i++) {
                if ($scope.countrylist[i].select == true) {
                    groupidss.push({ ivrmmC_Id: $scope.countrylist[i].ivrmmC_Id })
                }
            }
            if (groupidss != undefined) {
                var data = {
                    stateids: groupidss,
                };
                apiService.create("SalesSMSEMAIL/get_state", data).
                    then(function (promise) {

                        $scope.statelist = promise.statelist;


                    });
            }

        };
        $scope.snd_email = false;
        $scope.snd_sms = false;
        $scope.printstudents = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.leadlist, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        }

        $scope.optionToggled1 = function (SelectedStudentRecord, index) {

            $scope.all = $scope.leadlist.every(function (options) {

                return options.selected;
            });
        }

        $scope.viewfunction = function () {
            $scope.grid_view = false;
            $scope.upcomingintvw = [];
            $scope.inprogressintvw = [];
            $scope.completedintvw = [];
        };
        $scope.namesearch = '';
        //fill employee end
        //get report start
        $scope.grid_view = false;
        $scope.submitted = false;
        $scope.leadlist = [];
        $scope.GetReport = function () {
            $scope.leadlist = [];
            var catIds;
            for (var i = 0; i < $scope.categorylist.length; i++) {
                if ($scope.categorylist[i].housselect == true) {

                    if (catIds == undefined)
                        catIds = $scope.categorylist[i].ismsmcA_Id;
                    else
                        catIds = catIds + "," + $scope.categorylist[i].ismsmcA_Id;
                }
            }
            var soursIds;
            for (var i = 0; i < $scope.sourcelist.length; i++) {
                if ($scope.sourcelist[i].select == true) {

                    if (soursIds == undefined)
                        soursIds = $scope.sourcelist[i].ismsmsO_Id;
                    else
                        soursIds = soursIds + "," + $scope.sourcelist[i].ismsmsO_Id;
                }
            }
            var prodidss;
            for (var i = 0; i < $scope.productlist.length; i++) {
                if ($scope.productlist[i].select == true) {

                    if (prodidss == undefined)
                        prodidss = $scope.productlist[i].ismsmpR_Id;
                    else
                        prodidss = prodidss + "," + $scope.productlist[i].ismsmpR_Id;
                }
            }

            var statussidss;
            for (var i = 0; i < $scope.statuslist.length; i++) {
                if ($scope.statuslist[i].select == true) {

                    if (statussidss == undefined)
                        statussidss = $scope.statuslist[i].ismsmsT_Id;
                    else
                        statussidss = statussidss + "," + $scope.statuslist[i].ismsmsT_Id;
                }
            }
            var contryidss;
            for (var i = 0; i < $scope.countrylist.length; i++) {
                if ($scope.countrylist[i].select == true) {

                    if (contryidss == undefined)
                        contryidss = $scope.countrylist[i].ivrmmC_Id;
                    else
                        contryidss = contryidss + "," + $scope.countrylist[i].ivrmmC_Id;
                }
            }

            var stsidss;

            if ($scope.statelist != undefined && $scope.statelist != null) {
                for (var i = 0; i < $scope.statelist.length; i++) {
                    if ($scope.statelist[i].select == true) {

                        if (stsidss == undefined)
                            stsidss = $scope.statelist[i].ivrmmS_Id;
                        else
                            stsidss = stsidss + "," + $scope.statelist[i].ivrmmS_Id;
                    }
                }
            }



            if (stsidss == undefined && contryidss == undefined && catIds == undefined && soursIds == undefined && statussidss == undefined && prodidss == undefined && ($scope.namesearch == undefined || $scope.namesearch == '' || $scope.namesearch == null) && ($scope.contactname == undefined || $scope.contactname == '' || $scope.contactname == null) && ($scope.mobilesearch == undefined || $scope.mobilesearch == '' || $scope.mobilesearch == null) && ($scope.emailsearch == undefined || $scope.emailsearch == '' || $scope.emailsearch == null)) {
                swal('Select Atleast one set of search Parameter')
            }
            else {
                if ($scope.startdate != null && $scope.startdate != undefined) {
                    $scope.start_Date = $filter('date')($scope.startdate, "yyyy-MM-dd");
                }
                else {
                    $scope.start_Date = "";
                }
                if ($scope.enddate != null && $scope.enddate != undefined) {
                    $scope.end_Date = $filter('date')($scope.enddate, "yyyy-MM-dd");
                }
                else {
                    $scope.end_Date ="";
                }
                


                var data = {
                    "searchstring": $scope.namesearch,
                    "contactname": $scope.contactname,
                    "mobilesearch": $scope.mobilesearch,
                    "emailsearch": $scope.emailsearch,
                    "statussidss": statussidss,
                    "contryidss": contryidss,
                    "catIds": catIds,
                    "soursIds": soursIds,
                    "statidss": stsidss,
                    "prodidss": prodidss,
                    "start_Date": $scope.start_Date,
                    "end_Date": $scope.end_Date,
                };
                apiService.create("SalesSMSEMAIL/getrpt_lead", data).
                    then(function (promise) {

                        if (promise.leadlist.length > 0 && promise.leadlist != null) {
                            $scope.grid_view = true;

                            $scope.leadlist = promise.leadlist;
                            $scope.leadlist_1 = promise.leadlist;
                           
                        }
                        else {
                            swal('No Record Found')
                        }

                    });
            }
            //}
            //else {
            //    $scope.submitted = true;
            //}
        };


        $scope.printData = function () {

            if ($scope.leadlist_1 !== null && $scope.leadlist_1.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/Sports/HouseReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }
        //$scope.exportToExcel = function (export_id) {

        //    var exportHref = Excel.tableToExcel(export_id, 'sheet name');
        //    $timeout(function () {
        //        location.href = exportHref;
        //    }, 100);

        //};

        $scope.exportToExcel = function (tableId) {
            var excelname = "LEAD REPORT";
            excelname = excelname.toUpperCase() + '.xls';
            var printSectionId = tableId;
            if ($scope.leadlist !== null && $scope.leadlist.length > 0) {
                var exportHref = Excel.tableToExcel(printSectionId, 'LEAD REPORT');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelname;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
                //$state.reload();
            }
            else {
                swal("Please Select Records to be Exported");
            }

        };
        


      
        $scope.clearid = function () {
            $state.reload();
        };
        
    }
   
})();