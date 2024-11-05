(function () {
    'use strict';
    angular
        .module('app')
        .controller('ReligionCasteCategoryReport', ReligionCasteCategoryReport)

    ReligionCasteCategoryReport.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function ReligionCasteCategoryReport($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("ReligionCasteCategoryReport/loaddata", pageid).then(function (promise) {
               
                $scope.yearlist = promise.yearlist;
                $scope.classlist = promise.classlist;
                $scope.castecategorylist = promise.castecategorylist;
                $scope.religionlist = promise.religionlist;
                $scope.grid = true;
            })
        }
        $scope.onlycat = false;
        $scope.onlyrel = false;
        $scope.classnamearray1 = [];
        $scope.sepgrid = false;
        $scope.never = false;
        $scope.seperategrid = false;
        $scope.showdetails = function () {
            $scope.sepgrid = false;
            $scope.never = false;
            $scope.seperategrid = false;
           
            $scope.classnamearray1 = [];
            $scope.selectedclasslist = [];
            angular.forEach($scope.classlist, function (aa) {
                if (aa.cls == true) {
                    $scope.selectedclasslist.push({ ASMCL_Id: aa.asmcL_Id, cname: aa.asmcL_ClassName });
                }
            })
            $scope.selectedcastecategorylist = [];
            angular.forEach($scope.castecategorylist, function (bb) {
                if (bb.cls == true) {
                    $scope.selectedcastecategorylist.push({ IMCC_Id: bb.imcC_Id, catname: bb.imcC_CategoryName });
                }
            })
            $scope.repboth = false;
            $scope.selectedreligionlist = [];
            angular.forEach($scope.religionlist, function (cc) {
                if (cc.select1 == true) {
                    $scope.selectedreligionlist.push({ IVRMMR_Id: cc.ivrmmR_Id, relname: cc.ivrmmR_Name })
                }
            })


             if ($scope.myForm.$valid) {
                 if ($scope.selectedcastecategorylist.length == 0 && $scope.selectedreligionlist.length == 0) {
                     swal("Please Select Atleast CasteCategory OR Religion");
                 }
                 else {
                     if ($scope.format == '1') {
                         
                     }
                     else if ($scope.format == '2') {
                         $scope.seperategrid = true;
                     }

                     var data = {
                         "ASMAY_Id": $scope.ASMAY_Id,
                         selectedclasslist: $scope.selectedclasslist,
                         selectedcastecategorylist: $scope.selectedcastecategorylist,
                         selectedreligionlist: $scope.selectedreligionlist,

                     }
                     apiService.create("ReligionCasteCategoryReport/showdetails", data).then(function (promise) {
                     
                         $scope.onlycat = false;
                         $scope.onlyrel = false;
                         $scope.reportlist = promise.reportlist;
                         $scope.reportlist2 = promise.reportlist2;                     

                         if ($scope.reportlist2 == null && $scope.reportlist2 == undefined && $scope.reportlist == null && $scope.reportlist == undefined) {
                         }

                         if ($scope.reportlist == null && $scope.reportlist == undefined && $scope.reportlist2 != null && $scope.reportlist2.length > 0) {
                             $scope.rep1 = true;
                             $scope.grid = true;
                             $scope.onlyrel = true;
                         }

                         if ($scope.reportlist2 == null && $scope.reportlist2 == undefined && $scope.reportlist != null && $scope.reportlist.length > 0) {
                             $scope.rep2 = true;
                             $scope.grid = true;
                             $scope.onlycat = true;
                         }
                         $scope.sepgrid = false;
                         if ($scope.reportlist2 != null && $scope.reportlist2.length > 0 && $scope.reportlist != null && $scope.reportlist.length > 0 && $scope.seperategrid == true) {
                             $scope.sepgrid = true;
                         }

                         // category report
                         if (promise.reportlist != null) {
                             $scope.grid = false;
                             $scope.reportlist = promise.reportlist;
                             angular.forEach($scope.selectedcastecategorylist, function (a) {
                                 $scope.casteid = [];
                                 angular.forEach($scope.reportlist, function (c) {
                                     if (a.IMCC_Id == c.IMCC_Id) {
                                         $scope.casteid.push(c);
                                     }
                                 })
                                 a.list = $scope.casteid;
                             })
                             console.log($scope.selectedcastecategorylist);
                             $scope.classnamearray = [];
                             angular.forEach($scope.selectedclasslist, function (ab) {
                                 $scope.classnamearray.push({ ASMCL_Id: ab.ASMCL_Id, gender: 'Boys', gender2: 'Male' })
                                 $scope.classnamearray.push({ ASMCL_Id: ab.ASMCL_Id, gender: 'Girls', gender2: 'Female' })
                                 $scope.classnamearray.push({ ASMCL_Id: ab.ASMCL_Id, gender: 'Total', gender2: 'Total' })
                             })                            
                             $scope.classnamearray;
                         }
                         // religion   
                         if (promise.reportlist2 != null) {
                             $scope.grid = false;
                             $scope.reportlist2 = promise.reportlist2;
                             angular.forEach($scope.selectedreligionlist, function (a) {
                                 $scope.casteid2 = [];
                                 angular.forEach($scope.reportlist2, function (c) {
                                     if (a.IVRMMR_Id == c.IVRMMR_Id) {
                                         $scope.casteid2.push(c);
                                     }
                                 })
                                 a.list2 = $scope.casteid2;
                             })
                             console.log($scope.selectedreligionlist);

                             $scope.classnamearray = [];
                             angular.forEach($scope.selectedclasslist, function (ab) {
                                 $scope.classnamearray.push({ ASMCL_Id: ab.ASMCL_Id, gender: 'Boys', gender2: 'Male' })
                                 $scope.classnamearray.push({ ASMCL_Id: ab.ASMCL_Id, gender: 'Girls', gender2: 'Female' })
                                 $scope.classnamearray.push({ ASMCL_Id: ab.ASMCL_Id, gender: 'Total', gender2: 'Total' })
                             })
                         }

                         console.log($scope.classnamearray);
                         $scope.classnamearray1=[];
                         angular.forEach($scope.selectedclasslist, function (rr) {
                             var boyst = 0;
                             var grlst = 0;
                             var tt = 0;
                             angular.forEach($scope.classnamearray, function (xx) {
                                 
                               //  if (rr.ASMCL_Id == xx.ASMCL_Id) {
                                 angular.forEach(promise.reportlist , function (mm) {
                                         
                                         if (rr.ASMCL_Id == xx.ASMCL_Id && rr.ASMCL_Id == mm.ASMCL_Id) {
                                             
                                             if (xx.gender =='Boys') {
                                                 boyst = boyst + mm.boycount;
                                             }
                                             if (xx.gender == 'Girls') {
                                                 grlst = grlst + mm.girlcount;
                                             }
                                             if (xx.gender == 'Total') {
                                                 tt = tt + mm.totalcount;
                                             }
                                         }

                                     })
                               //  }
                             })
                             $scope.classnamearray1.push({ ASMCL_Id: rr.ASMCL_Id, gender: 'Boys', gender2: 'Male', tt: boyst})
                             $scope.classnamearray1.push({ ASMCL_Id: rr.ASMCL_Id, gender: 'Girls', gender2: 'Female', tt: grlst })
                             $scope.classnamearray1.push({ ASMCL_Id: rr.ASMCL_Id, gender: 'Total', gender2: 'Total', tt: tt  })
                         })
                         console.log($scope.classnamearray1);
                     })
                 }
            }
            else {
                $scope.Submitted = true;
            }
        }  

        $scope.getformat = function () {

        }

        $scope.search = "";
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.searchchkbx3 = "";
        $scope.searchchkbx4 = "";
        $scope.searchchkbxtype = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.all_check3 = function () {
            var toggleStatus = $scope.usercheck3;
            angular.forEach($scope.classlist, function (itm) {
                itm.cls = toggleStatus;
            });
           
        };
        $scope.all_check4 = function () {
            var toggleStatus = $scope.usercheck4;
            angular.forEach($scope.castecategorylist, function (itm) {
                itm.cls = toggleStatus;
            });            
        };
        $scope.all_checktype = function () {
            var checkStatus = $scope.userchecktype;
            angular.forEach($scope.religionlist, function (itmtype) {
                itmtype.select1 = checkStatus;
            })
        }
        $scope.togchkbx3 = function () {
            $scope.usercheck3 = $scope.classlist.every(function (options) {
                return options.cls;
            });
           
        };
        $scope.togchkbx4 = function () {
            $scope.usercheck4 = $scope.castecategorylist.every(function (options) {
                return options.cls;
            });
            
        };
        $scope.togchkbxtype = function () {
            $scope.userchecktype = $scope.religionlist.every(function (optionstype) {
                return optionstype.select1;
            })
        }
        $scope.isOptionsRequired3 = function () {
            return !$scope.classlist.some(function (options) {
                return options.cls;
            })
        }
        //$scope.isOptionsRequired4 = function () {
        //    return !$scope.castecategorylist.some(function (options) {
        //        return options.cls;
        //    })
        //}
        $scope.isOptionsRequiredtype = function () {
            return !$scope.religionlist.some(function (options) {
                return options.select1;
            })
        }
        //$scope.filterchkbx = function (obj) {
        //    return (angular.lowercase(obj.amB_BranchName)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        //}
        //$scope.filterchkbxtype = function (obj) {
        //    return (angular.lowercase(obj.amsE_SEMName)).indexOf(angular.lowercase($scope.searchchkbxtype)) >= 0;
        //}
        $scope.Submitted = false;
        $scope.interacted = function () {
            return $scope.Submitted;
        }
        $scope.showing = false;
        $scope.exportToExcel = function (table) {
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }
        $scope.printData = function () {  
                var innerContents = '';
                innerContents = document.getElementById("printareaId1").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); },900);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
        }

        // category
        $scope.exportToExcelcat = function (table) {
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }
        $scope.printDatacat = function () {
            var innerContents = '';
            innerContents = document.getElementById("printareaId1cat").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); },900);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        }
        // religion
        $scope.exportToExcelrel = function (table) {
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }
        $scope.printDatarel = function () {
            var innerContents = '';
            innerContents = document.getElementById("printareaId1rel").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); },900);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        }
        $scope.never = false;
        // for format 2
        $scope.exportToExcelseperate = function (table) {
          
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }
        $scope.printDataseperate = function () {
        
            var innerContents = '';
            innerContents = document.getElementById("printareaId1seperate").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); },900);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        }
        $scope.cancel = function () {
            $state.reload();
        }
    }
})();
