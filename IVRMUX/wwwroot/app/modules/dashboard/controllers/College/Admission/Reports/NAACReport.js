(function () {
    'use strict';
    angular
        .module('app')
        .controller('NAACReportController', NAACReportController)

    NAACReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel','$timeout']
    function NAACReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {


        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));

        if (ivrmcofigsettings != null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null) {
            if (admfigsettings.length > 0) {
                var logopath = admfigsettings[0].asC_Logo_Path;
            } else {
                var logopath = "";
            }
        } else {
            var logopath = "";
        }


        $scope.imgname = logopath;

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;


        $scope.BindData = function () {
            apiService.getDATA("NAACReport/getdetails").
                then(function (promise) {
                    $scope.acdlist = promise.acdlist;
                    $scope.courselist = promise.courselist;
                })
        };

        $scope.onselectradio = function () {
            $scope.ASMAY_Id = '';
            $scope.AMCO_Id = '';
            $scope.StdAdm = false;
            $scope.CatStd = false;
            $scope.StdEnrol = false;
            $scope.ProgOffer = false;
            $scope.DeptList = false;
            angular.forEach($scope.acdlist, function (option9) {
                option9.Selected = false;
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.acdlist.some(function (options) {
                return options.Selected;
            });
        }

        $scope.getVolumeSumnewadm = function (items) {
            //var qww = items.colns;
            //return qww
            //    .map(function (x) { return x.noseats; })
            //    .reduce(function (a, b) { return a + b; });
            return items
                .map(function (x) { return x.PG; })
                .reduce(function (a, b) { return a + b; });
        };


        $scope.submitted = false;

        $scope.onreport = function () {
            if ($scope.myForm.$valid) {
                $scope.albumNameArraycolumn = [];
                if ($scope.NAAC == 'StdAdm' || $scope.NAAC == 'CatStd') {
                    angular.forEach($scope.acdlist, function (role) {
                        if (!!role.Selected) $scope.albumNameArraycolumn.push(role);
                    })
                }
                else {
                    $scope.albumNameArraycolumn.push({ asmaY_Id: $scope.ASMAY_Id });
                }

                var courseid = "";
                var yearid = "";
                if ($scope.NAAC == 'CasteRep') {
                    courseid = $scope.AMCO_Id;
                }
                else {
                    courseid = 0;
                }

                var data = {

                    "TempararyArrayListcoloumn": $scope.albumNameArraycolumn,
                    "AMCO_Id": courseid,
                    "flag": $scope.NAAC
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("NAACReport/onreport", data).
                    then(function (promise) {

                        if ($scope.NAAC == 'StdAdm') {
                            $scope.StdAdm = true;
                            $scope.CatStd = false;
                            $scope.StdEnrol = false;
                            $scope.ProgOffer = false;
                            $scope.DeptList = false;
                            $scope.CasteRep = false;
                            $scope.columnhead = [];
                            $scope.columnhead = [
                                { name: 'Male', id: 1 },
                                { name: 'Female', id: 2 }
                            ];

                            $scope.column_list = $scope.albumNameArraycolumn;
                            $scope.uharray = [];

                            angular.forEach($scope.column_list, function (s) {
                                angular.forEach($scope.columnhead, function (s1) {
                                    $scope.uharray.push({ name: s1.name, id_: s1.id, asmaY_Id: s.asmaY_Id });
                                })
                            })

                            if (promise.datareport != null) {
                                $scope.main_list = promise.datareport;
                                $scope.print = true;
                            }

                            else {
                                swal('No Records Found');
                                $scope.StdAdm = false;
                                $scope.print = false;
                            }
                        }
                        else if ($scope.NAAC == 'CatStd') {
                            $scope.CatStd = true;
                            $scope.StdAdm = false;
                            $scope.StdEnrol = false;
                            $scope.ProgOffer = false;
                            $scope.DeptList = false;
                            $scope.CasteRep = false;
                            $scope.column_list = $scope.albumNameArraycolumn;
                            if (promise.datareport != null) {
                                $scope.main_list = promise.datareport;
                                $scope.print = true;
                            }

                            else {
                                swal('No Records Found');
                                $scope.CatStd = false;
                            }
                        }
                        else if ($scope.NAAC == 'StdEnrol') {
                            $scope.columnhead = [];
                            $scope.StdEnrol = true;
                            $scope.CatStd = false;
                            $scope.StdAdm = false;
                            $scope.CasteRep = false;

                            if (promise.datareport != null) {

                                $scope.uharray = [];
                                $scope.uharray1 = [];
                                $scope.main_list = promise.datareport;
                                if ($scope.main_list.length > 0) {
                                    $scope.print = true;
                                } else {
                                    $scope.print = false;
                                }

                                $scope.tempmain_list = promise.datareport;
                                $scope.column_list = promise.datareport2;
                                $scope.main_list1 = promise.datareport1;

                                angular.forEach($scope.main_list, function (dd) {
                                    var total_days = 0;
                                    angular.forEach($scope.column_list, function (dd1) {
                                        total_days += dd[dd1.amcoC_Name];
                                    })

                                    dd.total = total_days;
                                })
                            }
                            else {
                                swal('No Records Found');
                                $scope.StdEnrol = false;
                                $scope.print = false;
                            }
                        }
                        else if ($scope.NAAC == 'ProgOffer') {
                            $scope.ProgOffer = true;
                            $scope.CatStd = false;
                            $scope.StdAdm = false;
                            $scope.StdEnrol = false;
                            $scope.CasteRep = false;
                            if (promise.datareport != null) {
                                $scope.main_list = promise.datareport;
                                $scope.print = true;
                                console.log($scope.main_list);
                            }
                            else {
                                swal('No Records Found');
                                $scope.ProgOffer = false;
                                $scope.print = false;
                            }
                        }

                        else if ($scope.NAAC == 'DeptList') {
                            $scope.DeptList = true;
                            $scope.ProgOffer = false;
                            $scope.CatStd = false;
                            $scope.StdAdm = false;
                            $scope.StdEnrol = false;
                            $scope.CasteRep = false;
                            if (promise.datareport != null) {
                                $scope.main_list = promise.datareport;
                                $scope.print = true;
                            }
                            else {
                                swal('No Records Found');
                                $scope.ProgOffer = false;
                                $scope.print = false;
                            }
                        }
                        //else if ($scope.NAAC == 'CasteRep') {
                        //    $scope.CasteRep = true;
                        //    $scope.DeptList = false;
                        //    $scope.ProgOffer = false;
                        //    $scope.CatStd = false;
                        //    $scope.StdAdm = false;
                        //    $scope.StdEnrol = false;
                        //    if (promise.datareport != null) {
                        //        $scope.main_list = promise.datareport;
                        //        $scope.religion = promise.religion;
                        //        $scope.castecateogry = promise.castecateogry;
                        //        $scope.semester = promise.semester;
                        //        $scope.print = true;
                        //    }
                        //    else {
                        //        swal('No Records Found');
                        //        $scope.ProgOffer = false;
                        //        $scope.print = false;
                        //    }
                        //}
                        else if ($scope.NAAC == 'CasteRep') {
                            $scope.CasteRep = true;
                            $scope.DeptList = false;
                            $scope.ProgOffer = false;
                            $scope.CatStd = false;
                            $scope.StdAdm = false;
                            $scope.StdEnrol = false;
                            if (promise.datareport !== null) {

                                $scope.main_list = promise.datareport;
                                console.log($scope.main_list);
                                $scope.religion = promise.religion;
                                $scope.castecateogry = promise.castecateogry;
                                $scope.semester = promise.semester;
                                $scope.print = true;
                                $scope.newtmary = [];
                                
                                //angular.forEach($scope.religion, function (q1) {
                                //    angular.forEach($scope.castecateogry, function (q2) {
                                //        angular.forEach($scope.semester, function (q3) {
                                //            angular.forEach($scope.main_list, function (q4) {
                                //                if ((q1.ivrmmR_Id === q4.IVRMMR_Id) && (q2.imcC_Id === q4.IMCC_Id)) {
                                //                    if (q3.amsE_Year === q4.AMSE_Year) {
                                //                        $scope.newtmary.push(q4);
                                //                    }
                                //                    else {
                                //                        $scope.newtmary.push({ IVRMMR_Id: q1.ivrmmR_Id, IMCC_Id: q2.imcC_Id, AMSE_Year: q3.amsE_Year,boys:0,girls:0,total:0})
                                //                    }
                                //                }

                                //            });
                                //        });
                                //    });
                                //});

                                console.log($scope.newtmary);
                                $scope.semtm = [
                                    { name: 'Male', id: 1 },
                                    { name: 'Female', id: 2 },
                                    { name: 'Total', id: 3 }
                                ];


                                $scope.uharraycw = [];

                                angular.forEach($scope.semester, function (s) {
                                    angular.forEach($scope.semtm, function (s1) {
                                        $scope.uharraycw.push({ name: s1.name, id_: s1.id });
                                    });
                                });




                                $scope.temparry = [];
                                $scope.temparry1 = [];
                                angular.forEach($scope.religion, function (obj) {
                                    $scope.temparrytmp = [];
                                    //  $scope.temparry1 = [];
                                    angular.forEach($scope.main_list, function (obj1) {
                                        if (obj.ivrmmR_Id === obj1.IVRMMR_Id) {
                                            $scope.temparrytmp.push(obj1);
                                        }
                                    });

                                    if ($scope.temparrytmp.length !== 0) {
                                        $scope.temparry.push({ relign: obj.ivrmmR_Name, relgnid: obj.ivrmmR_Id, categary: $scope.temparrytmp });
                                    }

                                });
                                $scope.temparry1 = [];
                                angular.forEach($scope.temparry, function (qwe) {
                                    
                                    $scope.temparry1 = [];
                                    angular.forEach($scope.castecateogry, function (obj) {
                                        $scope.temparrycattmp = [];

                                        angular.forEach(qwe.categary, function (obj1) {
                                            if (obj.imcC_Id === obj1.IMCC_Id) {
                                                $scope.temparrycattmp.push(obj1);
                                            }
                                        });

                                        if ($scope.temparrycattmp.length !== 0) {
                                            angular.forEach($scope.temparrycattmp, function (qwee) {
                                                var v1 = qwee.boys;
                                                var v2 = qwee.girls;
                                                var v3 = qwee.total;
                                                $scope.attt = [v1, v2, v3];
                                                qwee.att = $scope.attt;

                                            });
                                            $scope.temparry1.push({ categrynm: obj.imcC_CategoryName, catid: obj.imcC_Id, categarylst: $scope.temparrycattmp });
                                            //$scope.temparry1.push({ $scope.temparrycattmp });

                                        }

                                    });

                                    qwe.catlst = $scope.temparry1;
                                    // qwe.push($scope.temparry1);

                                });
                                
                                angular.forEach($scope.temparry, function (qwe) {
                                    angular.forEach(qwe.catlst, function (qwe1) {
                                        $scope.attrp = [];
                                        angular.forEach(qwe1.categarylst, function (qwe2) {
                                            angular.forEach(qwe2.att, function (qwe3) {
                                                $scope.attrp.push(qwe3);
                                            });

                                        });
                                        //if ($scope.attrp.length < $scope.semester.length) {
                                        //    for (var i = $scope.attrp.length; i < $scope.semester.length; i++) {

                                        //    }
                                        //}
                                        qwe1.qttp = $scope.attrp;

                                    });
                                });

                                console.log($scope.temparry);
                            }
                            else {
                                swal('No Records Found');
                                $scope.ProgOffer = false;
                                $scope.print = false;
                            }
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.printData = function () {
            var innerContents = document.getElementById("table").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.cancel = function () {
            $scope.ASMAY_Id = '';
            $scope.AMCO_Id = '';
            $scope.NAAC = '';
            $scope.StdAdm = false;
            $scope.CatStd = false;
            $scope.StdEnrol = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";

            angular.forEach($scope.acdlist, function (option9) {
                option9.Selected = false;
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.onprint = function () {
            var data = "";
            if ($scope.NAAC == 'StdAdm') {
                data = 'tablestdadm';

            } else if ($scope.NAAC == 'CatStd') {
                data = 'tablestdcatstd';
            }
            else if ($scope.NAAC == 'StdEnrol') {
                data = 'tablestdStdEnrol1';
            }
            else if ($scope.NAAC == 'ProgOffer') {
                data = 'tablestdProgOffer';
            }
            else if ($scope.NAAC == 'DeptList') {
                data = 'tablestddept';
            }
            else {
                data ='tablestdProgOffercaste'
            }
            var innerContents = document.getElementById(data).innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.exportToExcel = function (tableId) {            
            var exportHref = Excel.tableToExcel('#tablestdProgOffercaste1', 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);               
            } 
    }

})();