(function () {
    'use strict';
    angular
        .module('app')
        .controller('TeresianReportController', TeresianReportController)

    TeresianReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function TeresianReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        $scope.report = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }


        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        var logopath = "";
        if (admfigsettings !== null) {
            if (admfigsettings.length > 0) {
                logopath = admfigsettings[0].asC_Logo_Path;
            } else {
                logopath = "";
            }
        } else {
            logopath = "";
        }

        $scope.imgname = logopath;
        $scope.albumNameArraycolumn = [];
        $scope.columnsTest = [];
        $scope.printstudents = [];
        $scope.searchValue = '';
        $scope.students = [];

        $scope.catreport = true;
        $scope.submitted = false;
        $scope.semtypedis = false;

        $scope.array = [{ id: 'I_Year', checked: true, value: 'I Year' },
        { id: 'II_Year', checked: true, value: 'II Year' },
        { id: 'III_Year', checked: true, value: 'III Year' }];




        $scope.radiobtn = function () {

            if ($scope.entry === "aidedunadied") {
                $scope.semtypedis = false;
                $scope.coursedis = true;
                $scope.branchdis = true;
                $scope.semdis = true;
                $scope.checkbok = true;
                $scope.catreport = true;
            }
            else if ($scope.entry === "categorycombination") {
                $scope.semtypedis = true;
                $scope.coursedis = false;
                $scope.branchdis = false;
                $scope.semdis = false;
                $scope.checkbok = true;
                $scope.catreport = true;
            }
            else if ($scope.entry === "iiisem") {
                $scope.semtypedis = true;
                $scope.coursedis = false;
                $scope.branchdis = false;
                $scope.semdis = false;
                $scope.checkbok = false;
                $scope.catreport = true;
            }
            else if ($scope.entry === "iiyear") {
                $scope.semtypedis = true;
                $scope.coursedis = false;
                $scope.branchdis = false;
                $scope.semdis = false;
                $scope.checkbok = false;
                $scope.catreport = true;
            } else if ($scope.entry === "iiiyear") {
                $scope.semtypedis = true;
                $scope.coursedis = false;
                $scope.branchdis = false;
                $scope.semdis = false;
                $scope.checkbok = false;
                $scope.catreport = true;
            }
        };

        $scope.BindData = function () {

            var id = 2;
            apiService.getURI("TeresianReport/getdetails", id).then(function (promise) {

                $scope.acdlist = promise.acdlist;
                $scope.seclist = promise.seclist;
                $scope.quotalist = promise.quotalist;
                $scope.checklist = promise.check_list;
                $scope.feegrouparray = promise.feegrouparray;
                $scope.mastercategory = promise.mastercategory;

            });
        };

        $scope.onselectAcdYear = function (ASMAY_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id
            };
            apiService.create("TeresianReport/onselectAcdYear", data).then(function (promise) {
                $scope.courselist = promise.courselist;

            });
        };

        $scope.onselectcategory = function (ASMAY_Id, AMCOC_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id,
                "AMCOC_Id": AMCOC_Id
            };
            apiService.create("TeresianReport/onselectcategory", data).then(function (promise) {
                $scope.courselist = promise.courselist;
            });
        };

        $scope.onselectCourse = function (ASMAY_Id, AMCO_Id) {
            var data = {
                "AMCO_Id": AMCO_Id,
                "ASMAY_Id": ASMAY_Id
            };
            apiService.create("TeresianReport/onselectCourse", data).then(function (promise) {
                $scope.branchlist = promise.branchlist;
            });
        };

        $scope.onselectBranch = function (ASMAY_Id, AMCO_Id, AMB_Id) {
            var data = {

                "ASMAY_Id": ASMAY_Id,
                "AMCO_Id": AMCO_Id,
                "AMB_Id": AMB_Id
            };
            apiService.create("TeresianReport/onselectBranch", data).then(function (promise) {
                $scope.semlist = promise.semlist;
            });
        };

        $scope.test = [];
        $scope.onreport = function () {
            $scope.all = false;
            if ($scope.myForm.$valid) {
                $scope.test = [];
                var data = "";
                if ($scope.entry === "aidedunadied") {
                    data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "Evenorodd": $scope.mfa,
                        "Flag": $scope.entry,
                        "AMCOC_Id": $scope.AMCOC_Id
                    };

                } else if ($scope.entry === "general") {

                    data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "AMB_Id": $scope.AMB_Id,
                        "AMSE_Id": $scope.AMSE_Id,
                        "ACMS_Id": $scope.ACMS_Id
                    };
                } else if ($scope.entry === "iiisem") {

                    $scope.albumNameArraycolumn = [];

                    angular.forEach($scope.feegrouparray, function (role) {
                        if (!!role.selected) $scope.albumNameArraycolumn.push(role);
                    });

                    data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "AMB_Id": $scope.AMB_Id,
                        "AMSE_Id": $scope.AMSE_Id,
                        "ACMS_Id": $scope.ACMS_Id,
                        "AMCOC_Id": $scope.ACQ_Id,
                        "Temp_FeeGroup": $scope.albumNameArraycolumn,
                        "Flag": $scope.entry
                    };
                }
                else if ($scope.entry === "iiyear") {

                    $scope.albumNameArraycolumnnew = [];

                    angular.forEach($scope.feegrouparray, function (role) {
                        if (!!role.selected) $scope.albumNameArraycolumnnew.push(role);
                    });

                    data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "AMB_Id": $scope.AMB_Id,
                        "AMSE_Id": $scope.AMSE_Id,
                        "ACMS_Id": $scope.ACMS_Id,
                        "AMCOC_Id": $scope.stdtype,
                        "Temp_FeeGroup": $scope.albumNameArraycolumnnew,
                        "Flag": $scope.entry
                    };
                } else if ($scope.entry === "iiiyear") {

                    $scope.albumNameArraycolumnnew = [];

                    angular.forEach($scope.feegrouparray, function (role) {
                        if (!!role.selected) $scope.albumNameArraycolumnnew.push(role);
                    });

                    data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "AMB_Id": $scope.AMB_Id,
                        "AMSE_Id": $scope.AMSE_Id,
                        "ACMS_Id": $scope.ACMS_Id,
                        "AMCOC_Id": $scope.stdtype,
                        "Temp_FeeGroup": $scope.albumNameArraycolumnnew,
                        "Flag": $scope.entry
                    };
                }
                else if ($scope.entry === "categorycombination") {

                    data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "AMB_Id": 0,
                        "AMSE_Id": $scope.AMSE_Id,
                        "Flag": $scope.entry
                    };
                }

                apiService.create("TeresianReport/onreport", data).then(function (promise) {

                    if ($scope.entry === "aidedunadied") {

                        $scope.test = promise.studentlist;
                        $scope.coursedetails = promise.getcourselist;
                        $scope.getbranchlist = promise.getbranchlist;

                        $scope.students = promise.studentlist;
                        $scope.presentCountgrid = $scope.students.length;

                        if ($scope.students.length > 0 && $scope.students != null && $scope.students != undefined) {
                            $scope.catreport = false;

                            if (promise.categorynoyear[0].amcO_NoOfYears == 3) {

                                angular.forEach($scope.coursedetails, function (csdetails) {

                                    var cls_secs = [];
                                    var status_s = [];
                                    $scope.status_saa = [];
                                    angular.forEach($scope.getbranchlist, function (brdetails) {

                                        if (csdetails.amcO_Id == brdetails.amcO_Id) {
                                            cls_secs.push(brdetails);

                                            var present_s = [];
                                            angular.forEach($scope.array, function (d) {
                                                var value = null;
                                                angular.forEach($scope.students, function (att) {
                                                    if (att.couse_id == csdetails.amcO_Id && att.branch_id == brdetails.amB_Id) {
                                                        value = att[d.value];
                                                    }
                                                });
                                                if (value == 0)
                                                    value = '--';
                                                present_s.push(value);
                                            });

                                            present_s.push("");
                                            status_s.push({ name: 'Present', flag: 'P', amcO_CourseName: csdetails.amcO_CourseName, att: present_s });
                                            brdetails.att = present_s;
                                        }
                                    });
                                    csdetails.secs = cls_secs;

                                    var pre_tot = [];

                                    angular.forEach($scope.array, function (d) {

                                        var value = null;
                                        angular.forEach($scope.students, function (att) {
                                            if (att.couse_id == csdetails.amcO_Id)
                                                value += parseInt(att[d.value]);
                                        });
                                        pre_tot.push(value);
                                    });

                                    var testval = 0;
                                    angular.forEach(pre_tot, function (ooobj) {
                                        testval += ooobj;
                                    });

                                    pre_tot.push(testval);
                                    console.log(pre_tot);
                                    csdetails.secs.push({ amB_BranchName: 'Total', flag: 'TP', att: pre_tot });
                                    console.log($scope.status_saa);

                                    //csdetails.secs.push($scope.status_saa);
                                });
                            }
                            else {

                                $scope.array = [{ id: 'I_Year', checked: true, value: 'I Year' },
                                { id: 'II_Year', checked: true, value: 'II Year' }];


                                angular.forEach($scope.coursedetails, function (csdetails) {

                                    var cls_secs = [];
                                    var status_s = [];
                                    $scope.status_saa = [];
                                    angular.forEach($scope.getbranchlist, function (brdetails) {

                                        if (csdetails.amcO_Id == brdetails.amcO_Id) {
                                            cls_secs.push(brdetails);

                                            var present_s = [];
                                            angular.forEach($scope.array, function (d) {
                                                var value = null;
                                                angular.forEach($scope.students, function (att) {
                                                    if (att.couse_id == csdetails.amcO_Id && att.branch_id == brdetails.amB_Id) {
                                                        value = att[d.value];
                                                    }

                                                });
                                                if (value == 0)
                                                    value = '--';
                                                present_s.push(value);
                                            });

                                            present_s.push("");
                                            status_s.push({ name: 'Present', flag: 'P', amcO_CourseName: csdetails.amcO_CourseName, att: present_s });
                                            brdetails.att = present_s;
                                        }
                                    });

                                    csdetails.secs = cls_secs;

                                    var pre_tot = [];

                                    angular.forEach($scope.array, function (d) {

                                        var value = null;
                                        angular.forEach($scope.students, function (att) {
                                            if (att.couse_id == csdetails.amcO_Id)
                                                value += parseInt(att[d.value]);
                                        });
                                        pre_tot.push(value);
                                    });

                                    var testval = 0;
                                    angular.forEach(pre_tot, function (ooobj) {
                                        testval += ooobj;
                                    });

                                    pre_tot.push(testval);
                                    console.log(pre_tot);
                                    csdetails.secs.push({ amB_BranchName: 'Total', flag: 'TP', att: pre_tot });
                                    console.log($scope.status_saa);

                                    //csdetails.secs.push($scope.status_saa);
                                });

                            }
                            $scope.catreport = false;
                            $scope.exp_excel_flag = false;
                            $scope.print_flag = false;
                            $scope.report = true;
                            console.log($scope.coursedetails);
                        }
                        else {
                            swal("No Records Found!");
                            $scope.catreport = true;
                            $scope.report = false;
                            $state.reload();
                        }
                    } else if ($scope.entry === "iiisem") {

                        if (promise.studentlist.length > 0) {
                            $scope.catreport = false;
                            $scope.exp_excel_flag = false;
                            $scope.print_flag = false;
                            $scope.studentlistiiisem = promise.studentlist;
                            console.log($scope.studentlistiiisem);

                            angular.forEach($scope.courselist, function (dd) {
                                if (parseInt(dd.amcO_Id) === parseInt($scope.AMCO_Id)) {
                                    $scope.coursename = dd.amcO_CourseName;
                                }
                            });

                            angular.forEach($scope.branchlist, function (dd) {
                                if (parseInt(dd.amB_Id) === parseInt($scope.AMB_Id)) {
                                    $scope.branchname = dd.amB_BranchName;
                                }
                            });

                        } else {
                            $scope.catreport = true;
                            $scope.report = false;
                            $state.reload();
                            swal("No Records Found");
                        }
                    } else if ($scope.entry === "iiyear") {

                        if (promise.studentlist.length > 0) {
                            $scope.catreport = false;
                            $scope.exp_excel_flag = false;
                            $scope.print_flag = false;
                            $scope.studentlistiiyear = promise.studentlist;
                            console.log($scope.studentlistiiyear);

                            angular.forEach($scope.courselist, function (dd) {
                                if (parseInt(dd.amcO_Id) === parseInt($scope.AMCO_Id)) {
                                    $scope.coursename = dd.amcO_CourseName;
                                }
                            });

                            angular.forEach($scope.branchlist, function (dd) {
                                if (parseInt(dd.amB_Id) === parseInt($scope.AMB_Id)) {
                                    $scope.branchname = dd.amB_BranchName;
                                }
                            });

                        } else {
                            $scope.catreport = true;
                            $scope.report = false;
                            $state.reload();
                            swal("No Records Found");
                        }
                    } else if ($scope.entry === "iiiyear") {

                        if (promise.studentlist.length > 0) {
                            $scope.catreport = false;
                            $scope.exp_excel_flag = false;
                            $scope.print_flag = false;
                            $scope.studentlistiiyear = promise.studentlist;
                            console.log($scope.studentlistiiyear);

                            angular.forEach($scope.courselist, function (dd) {
                                if (parseInt(dd.amcO_Id) === parseInt($scope.AMCO_Id)) {
                                    $scope.coursename = dd.amcO_CourseName;
                                }
                            });

                            angular.forEach($scope.branchlist, function (dd) {
                                if (parseInt(dd.amB_Id) === parseInt($scope.AMB_Id)) {
                                    $scope.branchname = dd.amB_BranchName;
                                }
                            });

                        } else {
                            $scope.catreport = true;
                            $scope.report = false;
                            $state.reload();
                            swal("No Records Found");
                        }
                    } else if ($scope.entry === "categorycombination") {

                        if (promise.studentlist.length > 0) {
                            $scope.catreport = false;
                            $scope.exp_excel_flag = false;
                            $scope.print_flag = false;
                            $scope.studentcategorycombination = promise.studentlist;
                            console.log($scope.studentcategorycombination);
                            $scope.overalltotals();
                            $scope.totalboy();
                            $scope.totalgirl();

                            angular.forEach($scope.acdlist, function (dd) {
                                if (parseInt(dd.asmaY_Id) === parseInt($scope.ASMAY_Id)) {
                                    $scope.year = dd.asmaY_Year;
                                }
                            });

                            angular.forEach($scope.courselist, function (dd) {
                                if (parseInt(dd.amcO_Id) === parseInt($scope.AMCO_Id)) {
                                    $scope.course = dd.amcO_CourseName;
                                }
                            });


                        } else {
                            $scope.catreport = true;
                            $scope.report = false;
                            $state.reload();
                            swal("No Records Found");
                        }
                    }


                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancel = function () {
            $scope.ASMAY_Id = '';
            $scope.AMCO_Id = '';
            $scope.AMB_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = '';
            $scope.quota = '';
            $scope.mfa = '';
            $scope.catreport = true;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };



        $scope.addColumn2 = function (role1) {
            $scope.detail_checked = $scope.feegrouparray.every(function (itm) { return itm.selected; });
            if (role1.selected === true) {
                $scope.albumNameArraycolumn.push(role1);
                var newCol = { fmG_Id: role1.fmG_Id, checked: true, fmG_GroupName: role1.fmG_GroupName };
                $scope.columnsTest.push(newCol);
            }
            else {
                var som = $scope.albumNameArraycolumn.indexOf(role1);
                $scope.columnsTest.splice($scope.albumNameArraycolumn.indexOf(role1), 1);
                $scope.albumNameArraycolumn.splice($scope.albumNameArraycolumn.indexOf(role1), 1);
            }
        };


        $scope.all_check = function () {
            var toggleStatus1 = $scope.detail_checked;
            angular.forEach($scope.feegrouparray, function (itm) {
                itm.selected = toggleStatus1;
            });
        };


        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired = function () {
            return !$scope.feegrouparray.some(function (options) {
                return options.selected;
            });
        };

        $scope.toggleAll = function () {

            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
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
        };

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
        };

        $scope.exportToExcel = function (export_id) {
            var exportHref = "";
            if ($scope.entry === "iiisem") {
                exportHref = Excel.tableToExcel('#printSectionId1', 'Teresian Report');
                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            } else if ($scope.entry === "aidedunadied") {
                exportHref = Excel.tableToExcel('#printSectionId', 'Teresian Report');
                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            } else if ($scope.entry === "iiyear") {
                exportHref = Excel.tableToExcel('#printSectionId2', 'Teresian Report');
                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            } else if ($scope.entry === "iiiyear") {
                exportHref = Excel.tableToExcel('#printSectionId21', 'Teresian Report');
                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            } else {
                exportHref = Excel.tableToExcel('#printSectionId3', 'Teresian Report');
                $timeout(function () {
                    location.href = exportHref;
                }, 100);

            }
        };

        $scope.printData = function () {
            var data = "";
            if ($scope.entry === "iiisem") {
                data = "printSectionId1";
            } else if ($scope.entry === "iiyear") {
                data = "printSectionId2";
            } else if ($scope.entry === "iiiyear") {
                data = "printSectionId21";
            }else if ($scope.entry === "categorycombination") {
                data = "printSectionId3";
            } else {
                data = "printSectionId";
            }
            var innerContents = document.getElementById(data).innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.totalboy = function () {
            var sumtotoalpresent = 0;
            for (var i = 0; i < $scope.studentcategorycombination.length; i++) {
                sumtotoalpresent = sumtotoalpresent + $scope.studentcategorycombination[i].boys;
            }
            $scope.totalboys = sumtotoalpresent;
        };

        $scope.totalgirl = function () {
            var sumtotoalpresent = 0;
            for (var i = 0; i < $scope.studentcategorycombination.length; i++) {
                sumtotoalpresent = sumtotoalpresent + $scope.studentcategorycombination[i].girls;
            }
            $scope.totalgirls = sumtotoalpresent;
        };

        $scope.overalltotals = function () {
            var sumtotoalpresent = 0;
            for (var i = 0; i < $scope.studentcategorycombination.length; i++) {
                sumtotoalpresent = sumtotoalpresent + $scope.studentcategorycombination[i].total;
            }
            $scope.overalltotal = sumtotoalpresent;
        };
    }

})();