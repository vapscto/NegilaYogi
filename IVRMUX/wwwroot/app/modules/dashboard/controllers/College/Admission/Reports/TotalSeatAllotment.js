(function () {
    'use strict';
    angular
        .module('app')
        .controller('TotalSeatAllotmentController', TotalSeatAllotmentController)

    TotalSeatAllotmentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$timeout', 'Excel']
    function TotalSeatAllotmentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $timeout, Excel) {


        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));

        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;


        $scope.printflg = false;


        $scope.BindData = function () {
            apiService.getDATA("StudentGeneralRegister/getdetails").
                then(function (promise) {

                    $scope.acdlist = promise.acdlist;
                    $scope.courselist = promise.courselist;
                    $scope.semlist = promise.semlist;
                    $scope.quotalist = promise.quotalist;

                })
        };


        $scope.all_check = function () {
            var toggleStatus = $scope.detail_checked;
            angular.forEach($scope.quotalist, function (itm) {
                itm.ivrm_id = toggleStatus;
            });
        }

        $scope.addColumn2 = function () {
            $scope.detail_checked = $scope.quotalist.every(function (options) {
                return options.ivrm_id;
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.quotalist.some(function (options) {
                return options.ivrm_id;
            });
        }

        $scope.submitted = false;

        //$scope.onreport = function () {
        //    if ($scope.myForm.$valid) {
        //        $scope.albumNameArraycolumn = [];
        //        angular.forEach($scope.quotalist, function (role) {
        //            if (!!role.ivrm_id) $scope.albumNameArraycolumn.push(role);
        //        })

        //        var data = {
        //            "ASMAY_Id": $scope.ASMAY_Id,
        //            "AMCO_Id": $scope.AMCO_Id,
        //            "AMSE_Id": $scope.AMSE_Id,
        //            "TempararyArrayListcoloumn": $scope.albumNameArraycolumn

        //        }

        //        var config = {
        //            headers: {
        //                'Content-Type': 'application/json;'
        //            }
        //        }
        //        apiService.create("TotalSeatAllotment/onreport", data).
        //            then(function (promise) {

        //                $scope.main_list = promise.datareport;

        //                if ($scope.main_list != null) {
        //                    $scope.instutiondetails = promise.instutiondetails;
        //                    $scope.collegename = $scope.instutiondetails[0].mI_Name;



        //                    angular.forEach($scope.acdlist, function (yy) {
        //                        if (yy.asmaY_Id == $scope.ASMAY_Id) {
        //                            $scope.year = yy.asmaY_Year;
        //                        }
        //                    })

        //                    angular.forEach($scope.courselist, function (cc) {
        //                        if (cc.amcO_Id == $scope.AMCO_Id) {
        //                            $scope.course = cc.amcO_CourseName;
        //                        }
        //                    })

        //                    angular.forEach($scope.semlist, function (ss) {
        //                        if (ss.amsE_Id == $scope.AMSE_Id) {
        //                            $scope.semeister = ss.amsE_SEMName;
        //                        }
        //                    })



        //                    $scope.columnhead = [];
        //                    $scope.columnhead = [
        //                        { name: 'Int', id: 1 },
        //                        { name: 'Adm', id: 2 },
        //                        { name: 'Vac', id: 3 }
        //                    ];

        //                    $scope.column_list = $scope.albumNameArraycolumn;
        //                    $scope.column_list.push({ acQ_Id: 0, acQ_QuotaName: 'Total' })
        //                    $scope.uharray = [];
        //                    angular.forEach($scope.column_list, function (s) {
        //                        angular.forEach($scope.columnhead, function (s1) {
        //                            $scope.uharray.push({ name: s1.name, id_: s1.id, acQ_Id: s.acQ_Id });
        //                        })
        //                    })


        //                    angular.forEach($scope.main_list, function (s1) {
        //                        s1.Int = s1.acscD_SeatNos;
        //                        s1.Adm = s1.count;
        //                        s1.Vac = s1.acscD_SeatNos - s1.count;
        //                    })

        //                    var temp_list = [];

        //                    angular.forEach($scope.main_list, function (s1) {
        //                        var list = [];
        //                        var cnt = 0;
        //                        angular.forEach(temp_list, function (sa) {
        //                            if (sa.amB_Id == s1.amB_Id) {
        //                                cnt += 1;
        //                            }
        //                        })

        //                        if (cnt == 0) {
        //                            angular.forEach($scope.main_list, function (s2) {
        //                                if (s1.amB_Id == s2.amB_Id) {
        //                                    list.push(s2);
        //                                }
        //                            })
        //                            var obj = {};
        //                            obj = s1;
        //                            obj.list = list;
        //                            temp_list.push(obj);
        //                            console.log(obj.list);
        //                        }
        //                    })


        //                    $scope.main_list = temp_list;
        //                    angular.forEach($scope.main_list, function (dd) {
        //                        var totalseats = 0;
        //                        angular.forEach($scope.uharray, function (ddd) {

        //                            angular.forEach(dd.list, function (dddd) {
        //                                if (dddd.acQ_Id == ddd.acQ_Id) {
        //                                    totalseats += dddd.Int;
        //                                }
        //                                dddd.name = totalseats;
        //                            })
        //                        })
        //                    })
        //                }
        //                else {
        //                    swal("No Records Found");
        //                }

        //                console.log($scope.main_list);
        //            })
        //    }
        //    else {
        //        $scope.submitted = true;
        //    }
        //}

        $scope.onreport = function () {

            if ($scope.myForm.$valid) {
                $scope.intake = 0;
                $scope.admitted = 0;
                $scope.vacancy = 0;

                $scope.albumNameArraycolumn = [];
                angular.forEach($scope.quotalist, function (role) {
                    if (!!role.ivrm_id) $scope.albumNameArraycolumn.push(role);
                })

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "TempararyArrayListcoloumn": $scope.albumNameArraycolumn

                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("TotalSeatAllotment/onreport", data).
                    then(function (promise) {

                        $scope.columnhead = [];
                        $scope.columnhead = [
                            { name: 'Int', id: 1 },
                            { name: 'Adm', id: 2 },
                            { name: 'Vac', id: 3 }
                        ];
                        $scope.column_list = $scope.albumNameArraycolumn;
                        $scope.uharray = [];
                        angular.forEach($scope.column_list, function (s) {
                            angular.forEach($scope.columnhead, function (s1) {
                                $scope.uharray.push({ name: s1.name, id_: s1.id, acQ_Id: s.acQ_Id });
                            })
                        })

                        $scope.main_list = promise.datareport;
                        $scope.main_list1 = promise.datareport1;
                        $scope.main_list2 = promise.datareport2;

                        $scope.uharray1 = [];
                        angular.forEach($scope.main_list2, function (s) {
                            angular.forEach($scope.columnhead, function (s1) {
                                $scope.uharray1.push({ name: s1.name, amB_Id: s.amb });
                            })
                        })

                        angular.forEach($scope.main_list2, function (s1) {

                            s1.Int = s1.totalseats;
                            s1.Adm = s1.admitted;
                            s1.Vac = s1.vac;

                        })

                        angular.forEach($scope.main_list, function (s1) {

                            s1.Int = s1.acscD_SeatNos;
                            s1.Adm = s1.count;
                            s1.Vac = s1.acscD_SeatNos - s1.count;

                        })

                        var temp_list = [];
                        angular.forEach($scope.main_list, function (s1) {
                            var list = [];
                            var cnt = 0;
                            angular.forEach(temp_list, function (sa) {
                                if (sa.amB_Id == s1.amB_Id) {
                                    cnt += 1;
                                }
                            })
                            if (cnt == 0) {
                                angular.forEach($scope.main_list, function (s2) {
                                    if (s1.amB_Id == s2.amB_Id) {
                                        list.push(s2);
                                    }
                                })
                                var obj = {};
                                obj = s1;
                                obj.list = list;
                                temp_list.push(obj);

                            }

                        })
                        $scope.main_list = temp_list;


                        //$scope.intake1 = 0;
                        //var admitted1, vacancy1 = 0;
                        //debugger;

                        //for (var j = 0; j < promise.datareport.length; j++) {
                        //    for (var i = 0; i < $scope.column_list.length; i++) {
                        //        if ($scope.column_list[i].acQ_Id == promise.datareport[j].acQ_Id) {
                        //            $scope.intake1 = $scope.intake1 + promise.datareport[i].acscD_SeatNos;
                        //        }                               
                        //    }
                        //}
                    })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.excelflg = false;
        $scope.exportToExcel = function (tableId) {
            $scope.excelflg = true;
            //if ($scope.main_list !== null && $scope.main_list.length > 0) {
            //    var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            //    $timeout(function () { location.href = exportHref; }, 100);
            //}
            if ($scope.main_list.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }

        }

        $scope.printData = function () {
            $scope.printflg = true;
            var innerContents = document.getElementById("tableId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.cancel = function () {
            $scope.ASMAY_Id = '';
            $scope.AMCO_Id = '';
            $scope.AMB_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACQ_Id = '';
            $scope.main_list = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";


            angular.forEach($scope.quotalist, function (option9) {
                option9.ivrm_id = false;
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

    }

})();