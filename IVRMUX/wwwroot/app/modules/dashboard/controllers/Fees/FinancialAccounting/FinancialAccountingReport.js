(function () {
    'use strict'; angular.module('app')
        .controller('FinancialAccountingReportController', FinancialAccountingReportController);
    FinancialAccountingReportController.$inject = ['$scope', '$state', 'apiService', '$http', 'Excel', '$timeout', '$filter', '$sce', '$location', '$anchorScroll'];
    function FinancialAccountingReportController($scope, $state, apiService, $http, Excel, $timeout, $filter, $sce, $location, $anchorScroll) {

        $('.modal').on('hide.bs.modal', function (e) {
            e.stopPropagation();
            $('body').css('padding-right', '');
        });

        $scope.scroll = function () {
            $location.hash('scrollid');
            $anchorScroll();
        }

        $scope.multidepartment = false;
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };










        $scope.collapseflag = " box box-primary";
        $scope.maxDatemf = new Date();
        $scope.reportstatus = false;
        $scope.fromdate = new Date();
        $scope.todate = new Date();
        $scope.reporttype = "E";
        $scope.submitted = false;
        $scope.exact = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.objupdate = {};
        $scope.nexttaskobj = {};
        $scope.searchValuecol = {};
        $scope.nexttaskobjm = {};
        $scope.reportdetails = [];
        $scope.bgcolorarray = ["#ffffff", "#ebe7e7", "#adadaf", "#ddddff", "#c2dfd8", "#b7b7b7", "#fde5e5", "#ecfde5", "#9daf95", "#efd7ac", "#9b9b9b"];

        $scope.reportdetailsbalancesheet = [];

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            var pageid = 2;
            apiService.getURI("FinancialAccountingReport/GetInitialData", pageid).
                then(function (promise) {

                    $scope.fillcompany = promise.fillcompany;
                    $scope.fyear = promise.fillfinacialyear;

                })
        };



        $scope.SearchEmployee = function () {
            $scope.liability = [];
            $scope.assets = [];
            $scope.levelcode = [];
            $scope.parentid = [];
            $scope.child = [];
            $scope.reportstatus = false;
            if ($scope.myForm.$valid) {
                var data = {
                    "IMFY_Id": $scope.IMFY_Id,
                    "FAMCOMP_Id": $scope.famcomP_Id,
                    "Fromdate": $scope.fromdate,
                    "Todate": $scope.todate,
                    "type": $scope.allind
                };
                apiService.create("FinancialAccountingReport/getreport", data).then(function (promise) {
                    if (promise.reportdetails.length > 0 && promise.reportdetails != null) {

                        $scope.reportstatus = true;
                        //$scope.collapseflag = "box box-primary collapsed-box";
                        $scope.arrowupdown = "fa fa-sort-desc";
                        if ($scope.allind == "Balance sheet") {

                            $scope.reportdetailsbalancesheet = promise.reportdetails



                            angular.forEach(promise.reportdetails, function (item) {
                                item.arrowupdown = "fa fa-sort-desc";
                                var al_cnt = 0;
                                angular.forEach($scope.levelcode, function (rt) {
                                    if (parseInt(rt.levelcodenew) === parseInt(item.levelcodenew)) {
                                        al_cnt += 1;
                                    }
                                });
                                if (al_cnt === 0) {


                                    $scope.levelcode.push({

                                        levelcodenew: item.levelcodenew,

                                        levelcode: 'Level' + item.levelcodenew,

                                    });
                                }

                                if (item.FAMGRP_ParentId == 0) {
                                    $scope.parentid.push(item);


                                    if (item.mg_type == "CR") {

                                        $scope.liability.push(item);
                                    }
                                    else {
                                        $scope.assets.push(item);
                                    }
                                }
                            });



                        }
                        else if ($scope.allind == "Profit And Loss") {

                            $scope.reportdetailsbalancesheet = promise.reportdetails


                            angular.forEach(promise.reportdetails, function (item) {
                                item.arrowupdown = "fa fa-sort-desc";
                                var al_cnt = 0;
                                angular.forEach($scope.levelcode, function (rt) {
                                    if (parseInt(rt.levelcodenew) === parseInt(item.levelcodenew)) {
                                        al_cnt += 1;
                                    }
                                });
                                if (al_cnt === 0) {


                                    $scope.levelcode.push({

                                        levelcodenew: item.levelcodenew,

                                        levelcode: 'Level' + item.levelcodenew,

                                    });
                                }

                                if (item.FAMGRP_ParentId == 0) {
                                    $scope.parentid.push(item);


                                    if (item.mg_type == "CR") {
                                        $scope.liability.push(item);
                                    }
                                    else {
                                        $scope.assets.push(item);
                                    }
                                }
                            });



                        }
                        else if ($scope.allind == "Bank/Cash Book") {
                            $scope.cashbookreport = [];
                            $scope.bankbookreport = [];

                            angular.forEach(promise.reportdetails, function (item) {
                                if (item.FAMGRP_GroupName === "Advances Received") {
                                    $scope.cashbookreport.push({ FAMGRP_Id: item.FAMGRP_Id, FAMLED_Id: item.FAMLED_Id, FAMGRP_GroupName: item.FAMGRP_GroupName, FAMLED_LedgerName: item.FAMLED_LedgerName, debit: item.debit, credit: item.credit });
                                }
                                else {
                                    $scope.bankbookreport.push({ FAMGRP_Id: item.FAMGRP_Id, FAMLED_Id: item.FAMLED_Id, FAMGRP_GroupName: item.FAMGRP_GroupName, FAMLED_LedgerName: item.FAMLED_LedgerName, debit: item.debit, credit: item.credit });
                                }
                            });


                        }
                        else if ($scope.allind == "Trail Balance") {

                            angular.forEach(promise.reportdetails, function (item) {
                                item.arrowupdown = "fa fa-sort-desc";
                                var al_cnt = 0;
                                angular.forEach($scope.levelcode, function (rt) {
                                    if (parseInt(rt.levelcodenew) === parseInt(item.levelcodenew)) {
                                        al_cnt += 1;
                                    }
                                });
                                if (al_cnt === 0) {

                                    $scope.levelcode.push({

                                        levelcodenew: item.levelcodenew,

                                        levelcode: 'Level' + item.levelcodenew,

                                    });
                                }

                                $scope.reportdetails.push(item);

                               
                            });

                        }
                        else {
                            $scope.reportdetails = promise.reportdetails
                        }



                    }
                    else {
                        swal("Data Is Not Found!!");
                        //$state.reload();
                        $scope.reportstatus = true;

                      

                    }
                });
            }
            else {
                $scope.submitted = true;
            }





        };


        $scope.gettodate = function () {

            $scope.minDatemf = new Date($scope.fromdate);
            $scope.maxDatemf = new Date();
        };


        $scope.subreport = function (item) {
            $scope.subreportdetails = [];
            if ($scope.myForm.$valid) {
                if (item.FAMVOU_Id > 0) {
                    var data = {
                        "FAMVOU_Id": item.FAMVOU_Id,
                        "IMFY_Id": $scope.IMFY_Id,
                        "Fromdate": $scope.fromdate,
                        "Todate": $scope.todate,
                        "reporttype": "daybookreport"
                    };
                }
                else if (item.FAMGRP_Id > 0) {
                    var data = {
                        "FAMGRP_Id": item.FAMGRP_Id,
                        "IMFY_Id": Number($scope.IMFY_Id),
                        "Fromdate": $scope.fromdate,
                        "Todate": $scope.todate,
                        "reporttype": "groupwiseledgerreport"
                    };
                }

                apiService.create("FinancialAccountingReport/subreport", data).then(function (promise) {
                    if (promise.subreportdetails.length > 0 && promise.subreportdetails != null) {
                        $scope.subreportdetails = promise.subreportdetails
                        if (item.FAMVOU_Id > 0) {
                            $scope.FAMVOU_VoucherType = item.FAMVOU_VoucherType;
                            $scope.FAMVOU_VoucherDate = item.FAMVOU_VoucherDate;
                            $scope.FAMVOU_VoucherNo = item.FAMVOU_VoucherNo;

                            $('#voucherpage').modal('show');
                        }
                        else if (item.FAMGRP_Id > 0) {

                            $scope.FAMGRP_GroupName = item.FAMGRP_GroupName;
                            $('#totalledgerpage').modal('show');
                        }
                    }
                    else {
                        swal("Data Is Not Found!!");
                        $scope.reportstatus = false;
                        // $state.reload();

                    }
                });
            }
            else {
                $scope.submitted = true;
            }





        };





        $scope.subreportBalancesheet = function (grpd, index) {
            var count = 0;

            if (grpd.arrowupdown == "fa fa-caret-up") {
                //  if (item.FAMGRP_ParentId == grpd.FAMGRP_Id) {
                count = count + 1;
                if (grpd.mg_type == "CR") {
                    $scope.liability.splice(index + 1, grpd.ChildRecords);
                }
                else {
                    $scope.assets.splice(index + 1, grpd.ChildRecords);
                }
                //}
            }
            else {

                angular.forEach($scope.reportdetailsbalancesheet, function (item) {


                    if (item.FAMGRP_ParentId == grpd.FAMGRP_Id) {
                        count = count + 1;
                        if (item.mg_type == "CR") {
                            item.gridbgcolor = $scope.bgcolorarray[count];
                            $scope.liability.splice(index + 1, 0, item);
                        }
                        else {
                            item.gridbgcolor = $scope.bgcolorarray[count];
                            $scope.assets.splice(index + 1, 0, item);
                        }
                    }





                });

            }
            

            angular.forEach($scope.reportdetailsbalancesheet, function (item) {
            if (item.FAMGRP_Id == grpd.FAMGRP_Id) {
                if (grpd.arrowupdown == "fa fa-caret-up") {
                    item.arrowupdown = "fa fa-sort-desc";
                }
                else {
                    item.arrowupdown = "fa fa-caret-up";
                }
                }
            });
            if (count == 0) {
                $scope.subreport(grpd);
            }

        };



        $scope.monthwisebalancereport = function (item) {
            if ($scope.myForm.$valid) {
                $scope.monthwisebalance = [];
                if (item.FAMLED_Id > 0) {
                    var data = {
                        "FAMLED_Id": item.FAMLED_Id,
                        "IMFY_Id": $scope.IMFY_Id,
                        "Fromdate": $scope.fromdate,
                        "Todate": $scope.todate,
                        "reporttype": "Monthwisebalancereport"
                    };
                }
                apiService.create("FinancialAccountingReport/subreport", data).then(function (promise) {
                    if (promise.subreportdetails.length > 0 && promise.subreportdetails != null) {
                        $scope.monthwisebalance = promise.subreportdetails
                        if (item.FAMLED_Id > 0) {
                            $scope.FAMLED_LedgerName = item.FAMLED_LedgerName;
                            $('#monthwisebalancepage').modal('show');
                        }
                    }
                    else {
                        swal("Data Is Not Found!!");
                        $scope.reportstatus = false;
                        // $state.reload();

                    }
                });
            }
            else {
                $scope.submitted = true;
            }





        };


        $scope.ledgerwisedaybookReport = function (item) {
            if ($scope.myForm.$valid) {
                $scope.ledgerwisedaybook = [];
                if (item.FAMLED_Id > 0) {
                    var data = {
                        "FAMLED_Id": item.FAMLED_Id,
                        "IMFY_Id": $scope.IMFY_Id,
                        "Fromdate": $scope.fromdate,
                        "Todate": $scope.todate,
                        "Monthid": item.MonthOrder,
                        "reporttype": "Ledgerwisereport"
                    };
                }
                apiService.create("FinancialAccountingReport/subreport", data).then(function (promise) {
                    if (promise.subreportdetails.length > 0 && promise.subreportdetails != null) {
                        $scope.ledgerwisedaybook = promise.subreportdetails
                        if (item.FAMLED_Id > 0) {
                            $('#ledgerwisedaybook').modal('show');
                        }
                    }
                    else {
                        swal("Data Is Not Found!!");
                        $scope.reportstatus = false;
                        // $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }





        };

        $scope.clearreport = function () {
            $scope.reportdetails = [];
            $scope.reportstatus = false;
        };

        $scope.OnLedger = function () {
            $scope.vocherlist = [];
            $('#voucherpage').modal('show');


            $scope.vocherlist.push({ drcrflag: "CR", perticulars: "Professional Charges ESI & EPF", debitamount: "0", creditamount: "2000" },
                { drcrflag: "DR", perticulars: "Professional Charges EPF", debitamount: "1000", creditamount: "0" },
                { drcrflag: "DR", perticulars: "Professional Charges  EPF", debitamount: "1000", creditamount: "0" });




            if ($scope.modaltasklist != null && $scope.modaltasklist.length > 0) {
                $('#voucherpage').modal('show');
            }
        };


        //*************************** GET REPORT *********************//        
        $scope.getinterdepartmentreport = function () {
            if ($scope.myForm.$valid) {
                if ($scope.reporttype == "E" || $scope.reporttype == "O") {
                    $scope.depid = $scope.Dept.hrmD_Id
                    $scope.departmentname = $scope.Dept.hrmD_DepartmentName;
                }
                else {
                    $scope.depid = 0;
                }
                if ($scope.departmentempissues != null && $scope.departmentempissues.length > 0) {
                    $scope.emparrayexist = true;
                }
                else {
                    $scope.emparrayexist = false;
                }
                if ($scope.departmentempissuesall != null && $scope.departmentempissuesall.length > 0) {
                    $scope.allarrayexist = true;
                }
                else {
                    $scope.allarrayexist = false;
                }
                var data = {
                    "startdate": new Date($scope.start_Date).toDateString(),
                    "enddate": new Date($scope.end_Date).toDateString(),
                    "reporttype": $scope.reporttype,
                    "HRMD_Id": $scope.depid,
                    "emparrayexist": $scope.emparrayexist,
                    "allarrayexist": $scope.allarrayexist
                };
                apiService.create("ISM_InterdepartmentIsuuesReport/getreport", data).then(function (promise) {
                    if (promise.successcount == 0) {
                        var countswal = 0;
                        //Consolidated Report
                        if (promise.consolidatedissues != null && promise.consolidatedissues.length > 0) {
                            $scope.consolidatedissueslist = promise.consolidatedissues;
                            $scope.consolidatedissues = [];
                            angular.forEach($scope.get_department, function (emp) {
                                var OpenCount = 0;
                                var Intermediate = 0;
                                var CloseCount = 0;
                                var PendingCount = 0;
                                angular.forEach($scope.consolidatedissueslist, function (issue) {
                                    if (emp.hrmD_Id == issue.AssgToDeptId) {
                                        OpenCount = OpenCount + issue.OpenCount;
                                        Intermediate = Intermediate + issue.Intermediate;
                                        CloseCount = CloseCount + issue.CloseCount;
                                        PendingCount = PendingCount + issue.PendingCount;
                                    }
                                });

                                $scope.consolidatedissues.push({ CreatedOnDepartment: emp.hrmD_DepartmentName, AssgToDeptId: emp.hrmD_Id, OpenCount: OpenCount, Intermediate: Intermediate, CloseCount: CloseCount, PendingCount: PendingCount })
                            });
                            $scope.consolidatedissuesview = promise.consolidatedissuesview;
                        }
                        else {
                            countswal = countswal + 1;
                        }

                        //InterDepartment Report
                        if (promise.interDepartmentissues != null && promise.interDepartmentissues.length > 0) {
                            $scope.interDepartmentissues = promise.interDepartmentissues;
                            $scope.interDepartmentissuesview = promise.interDepartmentissuesview;

                            $scope.createdbydepartmentlist = [];
                            var createdbydepartmentlist = [];
                            angular.forEach($scope.interDepartmentissues, function (value) {
                                createdbydepartmentlist.push(value.CreatedDeptId);
                            });

                            var filteredlistdept = createdbydepartmentlist.filter(function (value, index) { return createdbydepartmentlist.indexOf(value) == index });
                            $scope.interdeprtissuefilter = [];
                            angular.forEach(filteredlistdept, function (value) {
                                var filterceatedbyarr = $filter('filter')($scope.interDepartmentissues, function (d) {
                                    return d.CreatedDeptId === value;
                                });
                                if (filterceatedbyarr != null && filterceatedbyarr.length > 0) {
                                    $scope.interdeprtissuefilter.push({
                                        id: value, name: filterceatedbyarr[0].CreatedBYDepartment
                                    });
                                }
                            });
                        }
                        else {
                            countswal = countswal + 1;
                        }

                        //Employeewise Report
                        if (promise.departmentempissues != null && promise.departmentempissues.length > 0) {
                            $scope.departmentempissues = promise.departmentempissues;
                            $scope.departmentempissuesview = promise.departmentempissuesview;

                            $scope.statusfilteredlist = []; $scope.priorityfilteredlist = []; $scope.empfilteredlist = [];
                            var statuslist = []; var prioritylist = []; var emplist = [];
                            angular.forEach($scope.departmentempissues, function (value) {
                                statuslist.push(value.ISMTCR_Status);
                                prioritylist.push(value.Priority);
                                emplist.push(value.AssignedTo);
                            });

                            var statusfilteredlist = statuslist.filter(function (value, index) { return statuslist.indexOf(value) == index });
                            $scope.priorityfilteredlist = prioritylist.filter(function (value, index) { return prioritylist.indexOf(value) == index });
                            $scope.empfilteredlist = emplist.filter(function (value, index) { return emplist.indexOf(value) == index });

                            $scope.statusfilteredlist = [];
                            angular.forEach(statusfilteredlist, function (val) {
                                var count = 0;
                                angular.forEach($scope.departmentempissues, function (emp) {
                                    if (emp.ISMTCR_Status.toLowerCase() == val.toLowerCase()) {
                                        count = count + emp.Rcount;
                                    }
                                });
                                $scope.statusfilteredlist.push({ status: val, count: count });
                            });

                            $scope.statuswiseemployees = [];
                            angular.forEach($scope.empfilteredlist, function (emp) {
                                $scope.statuswiseemployees.push({ name: emp, subarray: [] });
                            });

                            angular.forEach($scope.statuswiseemployees, function (emp) {
                                var count = 0;
                                var pendingcount = 0;
                                var closecount = 0;
                                var opencount = 0;
                                angular.forEach($scope.departmentempissues, function (issue) {
                                    if (emp.name == issue.AssignedTo) {
                                        count = count + issue.Rcount;
                                        if (issue.ISMTCR_Status != null && issue.ISMTCR_Status != undefined) {
                                            if (issue.ISMTCR_Status.toLowerCase() != "close") {
                                                pendingcount = pendingcount + issue.Rcount;
                                            }
                                            if (issue.ISMTCR_Status.toLowerCase() == "close") {
                                                closecount = closecount + issue.Rcount;
                                            }
                                            if (issue.ISMTCR_Status.toLowerCase() == "open" || issue.ISMTCR_Status.toLowerCase() == "reopen") {
                                                opencount = opencount + issue.Rcount;
                                            }
                                        }
                                        emp.subarray.push({ status: issue.ISMTCR_Status, priority: issue.Priority, rcount: issue.Rcount, hrme_id: issue.AssignedToEmpId, departid: issue.AssgToDeptId });
                                        emp.empid = issue.AssignedToEmpId;
                                        emp.departid = issue.AssgToDeptId;
                                        emp.department = issue.CreatedOnDepartment;
                                    }
                                });
                                emp.count = count;
                                emp.pendingcount = pendingcount;
                                emp.closecount = closecount;
                                emp.opencount = opencount;
                            });
                        }
                        else {
                            countswal = countswal + 1;
                        }

                        //OverAll Individual Report
                        if (promise.overallempissues != null && promise.overallempissues.length > 0) {
                            $scope.overallempissues = promise.overallempissues;
                            $scope.overallempissuesempissuesview = promise.overallempissuesempissuesview;

                            $scope.ovrstatusfilteredlist = []; $scope.ovrpriorityfilteredlist = []; $scope.ovrempfilteredlist = [];
                            var ovrstatuslist = []; var ovrprioritylist = []; var ovremplist = [];
                            angular.forEach($scope.overallempissues, function (value) {
                                ovrstatuslist.push(value.ISMTCR_Status);
                                ovrprioritylist.push(value.Priority);
                                ovremplist.push(value.AssignedTo);
                            });

                            var ovrstatusfilteredlist = ovrstatuslist.filter(function (value, index) { return ovrstatuslist.indexOf(value) == index });
                            $scope.ovrpriorityfilteredlist = ovrprioritylist.filter(function (value, index) { return ovrprioritylist.indexOf(value) == index });
                            $scope.ovrempfilteredlist = ovremplist.filter(function (value, index) { return ovremplist.indexOf(value) == index });

                            $scope.ovrstatusfilteredlist = [];
                            angular.forEach(ovrstatusfilteredlist, function (val) {
                                var count = 0;
                                angular.forEach($scope.overallempissues, function (emp) {
                                    if (emp.ISMTCR_Status == val) {
                                        count = count + emp.Rcount;
                                    }
                                });
                                $scope.ovrstatusfilteredlist.push({ status: val, count: count });
                            });

                            $scope.ovrstatuswiseemployees = [];
                            angular.forEach($scope.ovrempfilteredlist, function (emp) {
                                $scope.ovrstatuswiseemployees.push({ name: emp, subarray: [] });
                            });

                            angular.forEach($scope.ovrstatuswiseemployees, function (emp) {
                                var count = 0;
                                var pendingcount = 0;
                                var closecount = 0;
                                var opencount = 0;
                                angular.forEach($scope.overallempissues, function (issue) {
                                    if (emp.name == issue.AssignedTo) {
                                        count = count + issue.Rcount;
                                        if (issue.ISMTCR_Status != null && issue.ISMTCR_Status != undefined) {
                                            if (issue.ISMTCR_Status.toLowerCase() != "close") {
                                                pendingcount = pendingcount + issue.Rcount;
                                            }
                                            if (issue.ISMTCR_Status.toLowerCase() == "close") {
                                                closecount = closecount + issue.Rcount;
                                            }
                                            if (issue.ISMTCR_Status.toLowerCase() == "open" || issue.ISMTCR_Status.toLowerCase() == "reopen") {
                                                opencount = opencount + issue.Rcount;
                                            }
                                        }
                                        emp.subarray.push({ status: issue.ISMTCR_Status, priority: issue.Priority, rcount: issue.Rcount, hrme_id: issue.AssignedToEmpId, departid: issue.AssgToDeptId });
                                        emp.empid = issue.AssignedToEmpId;
                                        emp.departid = issue.AssgToDeptId;
                                        emp.department = issue.CreatedOnDepartment;
                                    }
                                });
                                emp.count = count;
                                emp.pendingcount = pendingcount;
                                emp.closecount = closecount;
                                emp.opencount = opencount;
                            });
                        }
                        else {
                            countswal = countswal + 1;
                        }

                        if ((promise.departmentempissues != null && promise.departmentempissues.length > 0) && $scope.reporttype == "E") {
                            $scope.ovrallissuecountempwisetemp = [];
                            $scope.sumovrallissuecountempwise = [];
                            angular.forEach($scope.statuswiseemployees, function (emp) {
                                $scope.ovrallissuecountempwisetemp.push({ name: emp.name, hrme_id: emp.empid, ipending: emp.pendingcount, iclose: emp.closecount, itotal: emp.count, dpending: 0, dclose: 0, dtotal: 0, departid: emp.departid, iopen: emp.opencount, dopen: 0 })
                            });

                            $scope.totinterdeppending = 0; $scope.totinterdepclose = 0; $scope.totinterdeptot = 0; $scope.totinterdepopen = 0;
                            angular.forEach($scope.ovrallissuecountempwisetemp, function (emp) {
                                $scope.totinterdeppending = $scope.totinterdeppending + emp.ipending;
                                $scope.totinterdepclose = $scope.totinterdepclose + emp.iclose;
                                $scope.totinterdeptot = $scope.totinterdeptot + emp.itotal;
                                $scope.totinterdepopen = $scope.totinterdepopen + emp.iopen;
                            });
                        }
                        if ((promise.overallempissues != null && promise.overallempissues.length > 0) && $scope.reporttype == "O") {
                            $scope.ovrallissuecountempwise = [];
                            $scope.sumovrallissuecountempwise = [];
                            angular.forEach($scope.statuswiseemployees, function (emp) {
                                $scope.ovrallissuecountempwise.push({ name: emp.name, hrme_id: emp.empid, ipending: emp.pendingcount, iclose: emp.closecount, itotal: emp.count, dpending: 0, dclose: 0, dtotal: 0, departid: emp.departid, iopen: emp.opencount, dopen: 0 })
                            });

                            $scope.missedemployees = [];
                            angular.forEach($scope.ovrstatuswiseemployees, function (emp) {
                                var missemp = $filter('filter')($scope.ovrallissuecountempwise, function (d) {
                                    return (d.hrme_id === emp.empid);
                                });
                                if (missemp.length == 0) {
                                    $scope.ovrallissuecountempwise.push({ name: emp.name, hrme_id: emp.empid, ipending: 0, iclose: 0, itotal: 0, dpending: emp.pendingcount, dclose: emp.closecount, dtotal: emp.count, departid: emp.departid, iopen: 0, dopen: emp.opencount })
                                }
                            });

                            angular.forEach($scope.ovrstatuswiseemployees, function (emp) {
                                angular.forEach($scope.ovrallissuecountempwise, function (oemp) {
                                    if (emp.empid === oemp.hrme_id) {
                                        oemp.dpending = emp.pendingcount,
                                            oemp.dclose = emp.closecount,
                                            oemp.dtotal = emp.count,
                                            oemp.dopen = emp.opencount
                                    }
                                });
                            });

                            $scope.totinterdeppending = 0; $scope.totinterdepopen = 0; $scope.totinterdepclose = 0; $scope.totinterdeptot = 0; $scope.totdeppending = 0; $scope.totdepopen = 0;
                            $scope.totdepclose = 0; $scope.totdeptot = 0; $scope.totpending = 0; $scope.totclose = 0; $scope.total = 0; $scope.totopen = 0;
                            angular.forEach($scope.ovrallissuecountempwise, function (emp) {
                                $scope.totinterdeppending = $scope.totinterdeppending + emp.ipending;
                                $scope.totinterdepopen = $scope.totinterdepopen + emp.iopen;
                                $scope.totinterdepclose = $scope.totinterdepclose + emp.iclose;
                                $scope.totinterdeptot = $scope.totinterdeptot + emp.itotal;
                                $scope.totdeppending = $scope.totdeppending + emp.dpending;
                                $scope.totdepopen = $scope.totdepopen + emp.dopen;
                                $scope.totdepclose = $scope.totdepclose + emp.dclose;
                                $scope.totdeptot = $scope.totdeptot + emp.dtotal;
                                $scope.totpending = $scope.totpending + (emp.ipending + emp.dpending);
                                $scope.totclose = $scope.totclose + (emp.iclose + emp.dclose);
                                $scope.total = $scope.total + (emp.itotal + emp.dtotal);
                                $scope.totopen = $scope.totopen + (emp.iopen + emp.dopen);
                            });
                        }

                        //OverAll All Department Report                        
                        $scope.alldepartmentoverallpercentage = [];
                        if (promise.departmentempissuesall != null && promise.departmentempissuesall.length > 0) {
                            $scope.departmentempissuesall = promise.departmentempissuesall;
                            $scope.departmentempissuesviewall = promise.departmentempissuesviewall;
                            $scope.temp_epartmentempissuesall = promise.temp_epartmentempissuesall;

                            angular.forEach($scope.departmentempissuesall, function (emp) {
                                $scope.alldepartmentoverallpercentage.push({ name: emp.CreatedOnDepartment, departid: emp.AssgToDeptId, ipending: emp.PendingCount, iclose: emp.CloseCount, itotal: emp.Total, dpending: 0, dclose: 0, dtotal: 0, pending: 0, close: 0, total: 0 })
                            });
                        }
                        else {
                            countswal = countswal + 1;
                        }

                        if (promise.overallempissuesall != null && promise.overallempissuesall.length > 0) {
                            $scope.overallempissuesall = promise.overallempissuesall;
                            $scope.overallempissuesempissuesviewall = promise.overallempissuesempissuesviewall;
                            $scope.temp_overallempissuesall = promise.temp_overallempissuesall;


                            angular.forEach($scope.overallempissuesall, function (emp) {
                                var missemp = $filter('filter')($scope.alldepartmentoverallpercentage, function (d) {
                                    return (d.departid === emp.AssgToDeptId);
                                });
                                if (missemp.length == 0) {
                                    $scope.alldepartmentoverallpercentage.push({ name: emp.CreatedOnDepartment, departid: emp.AssgToDeptId, ipending: 0, iclose: 0, itotal: 0, dpending: emp.PendingCount, dclose: emp.CloseCount, dtotal: emp.Total, pending: 0, close: 0, total: 0 })
                                }
                            });

                            angular.forEach($scope.overallempissuesall, function (emp) {
                                angular.forEach($scope.alldepartmentoverallpercentage, function (oemp) {
                                    if (emp.AssgToDeptId === oemp.departid) {
                                        oemp.dpending = emp.PendingCount,
                                            oemp.dclose = emp.CloseCount,
                                            oemp.dtotal = emp.Total
                                    }
                                });
                            });

                            angular.forEach($scope.alldepartmentoverallpercentage, function (oemp) {
                                oemp.pending = oemp.ipending + oemp.dpending,
                                    oemp.close = oemp.iclose + oemp.dclose,
                                    oemp.total = oemp.itotal + oemp.dtotal
                            });

                        }
                        else {
                            countswal = countswal + 1;
                        }

                        if (countswal == 6) {
                            swal("No Records Found!!");
                        }
                    }
                    else {
                        swal("Please Regenerate Report!!");
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.filterfromall = function (getdept) {
            if (getdept != undefined && getdept != null) {
                $scope.clearonchange();
                if ($scope.temp_epartmentempissuesall != undefined && $scope.temp_epartmentempissuesall != null && $scope.temp_epartmentempissuesall.length > 0) {
                    $scope.departmentempissues = $filter('filter')($scope.temp_epartmentempissuesall, function (d) {
                        return (d.AssgToDeptId === getdept);
                    });

                    $scope.departmentempissuesview = $filter('filter')($scope.departmentempissuesviewall, function (d) {
                        return (d.AssgToDeptId === getdept);
                    });
                }
                if ($scope.temp_overallempissuesall != undefined && $scope.temp_overallempissuesall != null && $scope.temp_overallempissuesall.length > 0) {
                    $scope.overallempissues = $filter('filter')($scope.temp_overallempissuesall, function (d) {
                        return (d.AssgToDeptId === getdept);
                    });

                    $scope.overallempissuesempissuesview = $filter('filter')($scope.overallempissuesempissuesviewall, function (d) {
                        return (d.AssgToDeptId === getdept);
                    });
                }

                if ($scope.departmentempissues != null && $scope.departmentempissues.length > 0) {
                    $scope.departmentempissues = $scope.departmentempissues;
                    $scope.departmentempissuesview = $scope.departmentempissuesview;

                    $scope.statusfilteredlist = []; $scope.priorityfilteredlist = []; $scope.empfilteredlist = [];
                    var statuslist = []; var prioritylist = []; var emplist = [];
                    angular.forEach($scope.departmentempissues, function (value) {
                        statuslist.push(value.ISMTCR_Status);
                        prioritylist.push(value.Priority);
                        emplist.push(value.AssignedTo);
                    });

                    var statusfilteredlist = statuslist.filter(function (value, index) { return statuslist.indexOf(value) == index });
                    $scope.priorityfilteredlist = prioritylist.filter(function (value, index) { return prioritylist.indexOf(value) == index });
                    $scope.empfilteredlist = emplist.filter(function (value, index) { return emplist.indexOf(value) == index });

                    $scope.statusfilteredlist = [];
                    angular.forEach(statusfilteredlist, function (val) {
                        var count = 0;
                        angular.forEach($scope.departmentempissues, function (emp) {
                            if (emp.ISMTCR_Status != null && emp.ISMTCR_Status != undefined) {
                                if (emp.ISMTCR_Status.toLowerCase() == val.toLowerCase()) {
                                    count = count + emp.Rcount;
                                }
                            }
                        });
                        $scope.statusfilteredlist.push({ status: val, count: count });
                    });

                    $scope.statuswiseemployees = [];
                    angular.forEach($scope.empfilteredlist, function (emp) {
                        $scope.statuswiseemployees.push({ name: emp, subarray: [] });
                    });

                    angular.forEach($scope.statuswiseemployees, function (emp) {
                        var count = 0;
                        var pendingcount = 0;
                        var closecount = 0;
                        var opencount = 0;
                        angular.forEach($scope.departmentempissues, function (issue) {
                            if (emp.name == issue.AssignedTo) {
                                count = count + issue.Rcount;
                                if (issue.ISMTCR_Status != null && issue.ISMTCR_Status != undefined) {
                                    if (issue.ISMTCR_Status.toLowerCase() != "close") {
                                        pendingcount = pendingcount + issue.Rcount;
                                    }
                                    if (issue.ISMTCR_Status.toLowerCase() == "close") {
                                        closecount = closecount + issue.Rcount;
                                    }
                                    if (issue.ISMTCR_Status.toLowerCase() == "open" || issue.ISMTCR_Status.toLowerCase() == "reopen") {
                                        opencount = opencount + issue.Rcount;
                                    }
                                }
                                emp.subarray.push({ status: issue.ISMTCR_Status, priority: issue.Priority, rcount: issue.Rcount, hrme_id: issue.AssignedToEmpId, departid: issue.AssgToDeptId });
                                emp.empid = issue.AssignedToEmpId;
                                emp.departid = issue.AssgToDeptId;
                                emp.department = issue.CreatedOnDepartment;
                            }
                        });
                        emp.count = count;
                        emp.pendingcount = pendingcount;
                        emp.closecount = closecount;
                        emp.opencount = opencount;
                    });
                }

                if ($scope.overallempissues != null && $scope.overallempissues.length > 0) {
                    $scope.overallempissues = $scope.overallempissues;
                    $scope.overallempissuesempissuesview = $scope.overallempissuesempissuesview;

                    $scope.ovrstatusfilteredlist = []; $scope.ovrpriorityfilteredlist = []; $scope.ovrempfilteredlist = [];
                    var ovrstatuslist = []; var ovrprioritylist = []; var ovremplist = [];
                    angular.forEach($scope.overallempissues, function (value) {
                        ovrstatuslist.push(value.ISMTCR_Status);
                        ovrprioritylist.push(value.Priority);
                        ovremplist.push(value.AssignedTo);
                    });

                    var ovrstatusfilteredlist = ovrstatuslist.filter(function (value, index) { return ovrstatuslist.indexOf(value) == index });
                    $scope.ovrpriorityfilteredlist = ovrprioritylist.filter(function (value, index) { return ovrprioritylist.indexOf(value) == index });
                    $scope.ovrempfilteredlist = ovremplist.filter(function (value, index) { return ovremplist.indexOf(value) == index });

                    $scope.ovrstatusfilteredlist = [];
                    angular.forEach(ovrstatusfilteredlist, function (val) {
                        var count = 0;
                        angular.forEach($scope.overallempissues, function (emp) {
                            if (emp.ISMTCR_Status == val) {
                                count = count + emp.Rcount;
                            }
                        });
                        $scope.ovrstatusfilteredlist.push({ status: val, count: count });
                    });

                    $scope.ovrstatuswiseemployees = [];
                    angular.forEach($scope.ovrempfilteredlist, function (emp) {
                        $scope.ovrstatuswiseemployees.push({ name: emp, subarray: [] });
                    });

                    angular.forEach($scope.ovrstatuswiseemployees, function (emp) {
                        var count = 0;
                        var pendingcount = 0;
                        var closecount = 0;
                        var opencount = 0;
                        angular.forEach($scope.overallempissues, function (issue) {
                            if (emp.name == issue.AssignedTo) {
                                count = count + issue.Rcount;
                                if (issue.ISMTCR_Status != null && issue.ISMTCR_Status != undefined) {
                                    if (issue.ISMTCR_Status.toLowerCase() != "close") {
                                        pendingcount = pendingcount + issue.Rcount;
                                    }
                                    if (issue.ISMTCR_Status.toLowerCase() == "close") {
                                        closecount = closecount + issue.Rcount;
                                    }
                                    if (issue.ISMTCR_Status.toLowerCase() == "open" || issue.ISMTCR_Status.toLowerCase() == "reopen") {
                                        opencount = opencount + issue.Rcount;
                                    }
                                }
                                emp.subarray.push({ status: issue.ISMTCR_Status, priority: issue.Priority, rcount: issue.Rcount, hrme_id: issue.AssignedToEmpId, departid: issue.AssgToDeptId });
                                emp.empid = issue.AssignedToEmpId;
                                emp.departid = issue.AssgToDeptId;
                                emp.department = issue.CreatedOnDepartment;
                            }
                        });
                        emp.count = count;
                        emp.pendingcount = pendingcount;
                        emp.closecount = closecount;
                        emp.opencount = opencount;
                    });
                }

                if (($scope.departmentempissues != null && $scope.departmentempissues.length > 0)) {
                    $scope.ovrallissuecountempwisetemp = [];
                    $scope.sumovrallissuecountempwise = [];
                    angular.forEach($scope.statuswiseemployees, function (emp) {
                        $scope.ovrallissuecountempwisetemp.push({ name: emp.name, hrme_id: emp.empid, ipending: emp.pendingcount, iclose: emp.closecount, itotal: emp.count, dpending: 0, dclose: 0, dtotal: 0, departid: emp.departid, iopen: emp.opencount, dopen: 0 })
                    });

                    $scope.totinterdeppending = 0; $scope.totinterdepclose = 0; $scope.totinterdeptot = 0; $scope.totinterdepopen = 0;
                    angular.forEach($scope.ovrallissuecountempwisetemp, function (emp) {
                        $scope.totinterdeppending = $scope.totinterdeppending + emp.ipending;
                        $scope.totinterdepclose = $scope.totinterdepclose + emp.iclose;
                        $scope.totinterdeptot = $scope.totinterdeptot + emp.itotal;
                        $scope.totinterdepopen = $scope.totinterdepopen + emp.iopen;
                    });
                }
                if (($scope.overallempissues != null && $scope.overallempissues.length > 0)) {
                    $scope.ovrallissuecountempwise = [];
                    $scope.sumovrallissuecountempwise = [];
                    angular.forEach($scope.statuswiseemployees, function (emp) {
                        $scope.ovrallissuecountempwise.push({ name: emp.name, hrme_id: emp.empid, ipending: emp.pendingcount, iclose: emp.closecount, itotal: emp.count, dpending: 0, dclose: 0, dtotal: 0, departid: emp.departid, iopen: emp.opencount, dopen: 0 })
                    });

                    $scope.missedemployees = [];
                    angular.forEach($scope.ovrstatuswiseemployees, function (emp) {
                        var missemp = $filter('filter')($scope.ovrallissuecountempwise, function (d) {
                            return (d.hrme_id === emp.empid);
                        });
                        if (missemp.length == 0) {
                            $scope.ovrallissuecountempwise.push({ name: emp.name, hrme_id: emp.empid, ipending: 0, iclose: 0, itotal: 0, dpending: emp.pendingcount, dclose: emp.closecount, dtotal: emp.count, departid: emp.departid, iopen: 0, dopen: emp.opencount })
                        }
                    });

                    angular.forEach($scope.ovrstatuswiseemployees, function (emp) {
                        angular.forEach($scope.ovrallissuecountempwise, function (oemp) {
                            if (emp.empid === oemp.hrme_id) {
                                oemp.dpending = emp.pendingcount,
                                    oemp.dclose = emp.closecount,
                                    oemp.dtotal = emp.count,
                                    oemp.dopen = emp.opencount
                            }
                        });
                    });

                    $scope.totinterdeppending = 0; $scope.totinterdepopen = 0; $scope.totinterdepclose = 0; $scope.totinterdeptot = 0; $scope.totdeppending = 0; $scope.totdepopen = 0;
                    $scope.totdepclose = 0; $scope.totdeptot = 0; $scope.totpending = 0; $scope.totclose = 0; $scope.total = 0; $scope.totopen = 0;
                    angular.forEach($scope.ovrallissuecountempwise, function (emp) {
                        $scope.totinterdeppending = $scope.totinterdeppending + emp.ipending;
                        $scope.totinterdepopen = $scope.totinterdepopen + emp.iopen;
                        $scope.totinterdepclose = $scope.totinterdepclose + emp.iclose;
                        $scope.totinterdeptot = $scope.totinterdeptot + emp.itotal;
                        $scope.totdeppending = $scope.totdeppending + emp.dpending;
                        $scope.totdepopen = $scope.totdepopen + emp.dopen;
                        $scope.totdepclose = $scope.totdepclose + emp.dclose;
                        $scope.totdeptot = $scope.totdeptot + emp.dtotal;
                        $scope.totpending = $scope.totpending + (emp.ipending + emp.dpending);
                        $scope.totclose = $scope.totclose + (emp.iclose + emp.dclose);
                        $scope.total = $scope.total + (emp.itotal + emp.dtotal);
                        $scope.totopen = $scope.totopen + (emp.iopen + emp.dopen);
                    });
                }

                $scope.reporttype = "O";
                var getonedepartment = $filter('filter')($scope.get_department, function (d) {
                    return (d.hrmD_Id === getdept);
                });
                $scope.Dept = getonedepartment[0];
                $scope.departmentname = getonedepartment[0].hrmD_DepartmentName;
                $scope.scroll();
            }
        };



        $scope.oncountclick = function (empid, prio, status, depart) {
            $scope.modaltasklist = [];
            $scope.currentPage = 1;
            $scope.clearsearch();
            $scope.modaltasklist = $filter('filter')($scope.departmentempissuesview, function (d) {
                return (d.AssignedToEmpId === empid && d.Priority === prio && d.ISMTCR_Status === status && d.AssgToDeptId === depart);
            });
            if ($scope.modaltasklist != null && $scope.modaltasklist.length > 0) {
                $scope.empname = $scope.modaltasklist[0].AssignedTo.toUpperCase();
                $scope.empstatus = status;
                $scope.emppriority = prio;
                $scope.departview = $scope.Dept.hrmD_DepartmentName;
                $scope.filetrsatuscount();
                $('#myModalPreview').modal('show');
            }
        };

        $scope.onclickstatus = function (status) {
            $scope.modaltasklist = [];
            $scope.currentPage = 1;
            $scope.clearsearch();
            $scope.modaltasklist = $filter('filter')($scope.departmentempissuesview, function (d) {
                return (d.ISMTCR_Status === status && d.AssgToDeptId === $scope.Dept.hrmD_Id);
            });
            if ($scope.modaltasklist != null && $scope.modaltasklist.length > 0) {
                $scope.empname = "";
                $scope.empstatus = status;
                $scope.emppriority = "";
                $scope.departview = $scope.Dept.hrmD_DepartmentName;
                $scope.filetrsatuscount();
                $('#myModalPreview').modal('show');
            }
        };

        $scope.onstatusclickmodal = function (status) {
            $scope.exact = true;
            $scope.searchValuecol.ISMTCR_Status = "";
            $scope.searchValuecol.ISMTCR_Status = status;
            $scope.filterValue = $filter('filter')($scope.filterValue, function (d) {
                return (d.ISMTCR_Status === status);
            });
        };

        $scope.onstatusclickmodalnoft = function (status) {
            $scope.clearsearch();
            $scope.exact = true;
            $scope.searchValuecol.ISMTCR_Status = "";
            $scope.searchValuecol.ISMTCR_Status = status;
            $scope.filterValue = $filter('filter')($scope.modaltasklist, function (d) {
                return (d.ISMTCR_Status === status);
            });
        };

        $scope.modalcount = function () {
            if ($scope.modaltasklist.length > 0) {
                $scope.filtercreateduserlist();
                $scope.filterassigneduserlist();
                $scope.filterclientf();
                $scope.filtermodulef();
                $scope.filtertypef();
                $('#Modalcount').modal('show');
            } else {
                $('#Modalcount').modal('hide');
            }
        };

        $scope.clearindfltr = function (fltr) {
            $scope.exact = false;
            if (fltr == 1) {
                $scope.searchValuecol.ISMTCR_Status = "";
            }
            if (fltr == 2) {
                $scope.searchValuecol.Priority = "";
            }
            if (fltr == 3) {
                $scope.searchValuecol.Createdby = "";
            }
            if (fltr == 4) {
                $scope.searchValuecol.AssignedBy = "";
            }
            if (fltr == 5) {
                $scope.searchValuecol.AssignedTo = "";
            }
            if (fltr == 6) {
                $scope.searchValueall = '';
            }
        };

        $scope.filtercreateduserlist = function () {
            $scope.modallistcreated = [];
            var modallistcreated = $scope.modaltasklist.filter((item, i, arr) => arr.findIndex((t) => t.Createdby === item.Createdby) === i);
            angular.forEach(modallistcreated, function (value) {
                var count = $filter('filter')($scope.modaltasklist, function (d) {
                    return (d.Createdby === value.Createdby);
                });
                $scope.modallistcreated.push({ createdby: value.Createdby, count: count.length });
            });
        }

        $scope.filterassigneduserlist = function () {
            $scope.modallistassigned = [];
            var modallistassigned = $scope.modaltasklist.filter((item, i, arr) => arr.findIndex((t) => t.AssignedTo === item.AssignedTo) === i);
            angular.forEach(modallistassigned, function (value) {
                var count = $filter('filter')($scope.modaltasklist, function (d) {
                    return (d.AssignedTo === value.AssignedTo);
                });
                $scope.modallistassigned.push({ AssignedTo: value.AssignedTo, count: count.length });
            });
        }

        $scope.filterclientf = function () {
            $scope.modallistclient = [];
            var modallistclient = $scope.modaltasklist.filter((item, i, arr) => arr.findIndex((t) => t.client === item.client) === i);
            angular.forEach(modallistclient, function (value) {
                var count = $filter('filter')($scope.modaltasklist, function (d) {
                    return (d.client === value.client);
                });
                $scope.modallistclient.push({ client: value.client, count: count.length });
            });
        }

        $scope.filtermodulef = function () {
            $scope.filtermodule = [];
            var filtermodule = $scope.modaltasklist.filter((item, i, arr) => arr.findIndex((t) => t.module === item.module) === i);
            angular.forEach(filtermodule, function (value) {
                var count = $filter('filter')($scope.modaltasklist, function (d) {
                    return (d.module === value.module);
                });
                $scope.filtermodule.push({ module: value.module, count: count.length });
            });
        }

        $scope.filtertypef = function () {
            $scope.filtertype = [];
            var filtertype = $scope.modaltasklist.filter((item, i, arr) => arr.findIndex((t) => t.tasktype === item.tasktype) === i);
            angular.forEach(filtertype, function (value) {
                var count = $filter('filter')($scope.modaltasklist, function (d) {
                    return (d.tasktype === value.tasktype);
                });
                $scope.filtertype.push({ tasktype: value.tasktype, count: count.length });
            });
        }

        $scope.clickucreatedby = function (createdby, status) {
            $scope.clearsearch();
            $('#Modalcount').modal('hide');
            $scope.searchValuecol.Createdby = createdby;
            if (status != '') {
                $scope.exact = true;
                $scope.searchValuecol.ISMTCR_Status = status;
            }
            else {
                $scope.searchValuecol.ISMTCR_Status = "";
            }
        };

        $scope.clickuassignedto = function (AssignedTo, status) {
            $scope.clearsearch();
            $('#Modalcount').modal('hide');
            $scope.searchValuecol.AssignedTo = AssignedTo;
            if (status != '') {
                $scope.exact = true;
                $scope.searchValuecol.ISMTCR_Status = status;
            }
            else {
                $scope.searchValuecol.ISMTCR_Status = "";
            }
        };

        $scope.clickclient = function (cleint) {
            $scope.clearsearch();
            $scope.exact = false;
            $('#Modalcount').modal('hide');
            $scope.searchValueall = cleint;
        };

        $scope.clickmodule = function (module) {
            $scope.clearsearch();
            $scope.exact = false;
            $('#Modalcount').modal('hide');
            $scope.searchValueall = module;
        };

        $scope.clickutask = function (task) {
            $scope.exact = false;
            $scope.clearsearch();
            $('#Modalcount').modal('hide');
            $scope.searchValueall = task;
        };

        $scope.emptotal = function (empid, depart, type) {
            $scope.modaltasklist = [];
            $scope.modaltasklist1 = [];
            $scope.clearsearch();
            $scope.currentPage = 1;
            if (type == 1) {
                $scope.overalltype = "Inter-Departmental";
                $scope.modaltasklist = $filter('filter')($scope.departmentempissuesview, function (d) {
                    return (d.AssignedToEmpId === empid && d.AssgToDeptId === depart);
                });
            }
            else if (type == 2) {
                $scope.overalltype = "Department";
                $scope.modaltasklist = $filter('filter')($scope.overallempissuesempissuesview, function (d) {
                    return (d.AssignedToEmpId === empid && d.AssgToDeptId === depart);
                });
            }
            else if (type == 3) {
                $scope.overalltype = "OverAll";
                $scope.modaltasklist = $filter('filter')($scope.departmentempissuesview, function (d) {
                    return (d.AssignedToEmpId === empid && d.AssgToDeptId === depart);
                });
                if ($scope.reporttype == "O") {
                    $scope.modaltasklist1 = $filter('filter')($scope.overallempissuesempissuesview, function (d) {
                        return (d.AssignedToEmpId === empid && d.AssgToDeptId === depart);
                    });
                    angular.forEach($scope.modaltasklist1, function (emp) {
                        $scope.modaltasklist.push(emp);
                    });
                }
            }
            if ($scope.modaltasklist != null && $scope.modaltasklist.length > 0) {
                $scope.empname = $scope.modaltasklist[0].AssignedTo;
                $scope.empstatus = "Total";
                $scope.emppriority = "";
                $scope.departview = $scope.Dept.hrmD_DepartmentName;
                $scope.filetrsatuscount();
                $('#myModalPreview').modal('show');
            }
        };

        $scope.emppening = function (empid, depart, type) {
            $scope.modaltasklist = [];
            $scope.modaltasklist1 = [];
            $scope.currentPage = 1;
            $scope.clearsearch();
            if (type == 1) {
                $scope.overalltype = "Inter-Departmental";
                $scope.modaltasklist = $filter('filter')($scope.departmentempissuesview, function (d) {
                    return (d.AssignedToEmpId === empid && d.ISMTCR_Status.toLowerCase() !== "close" && d.AssgToDeptId === depart);
                });
            }
            else if (type == 2) {
                $scope.overalltype = "Department";
                $scope.modaltasklist = $filter('filter')($scope.overallempissuesempissuesview, function (d) {
                    return (d.AssignedToEmpId === empid && d.ISMTCR_Status.toLowerCase() !== "close" && d.AssgToDeptId === depart);
                });
            }
            else if (type == 3) {
                $scope.overalltype = "OverAll";
                $scope.modaltasklist = $filter('filter')($scope.departmentempissuesview, function (d) {
                    return (d.AssignedToEmpId === empid && d.ISMTCR_Status.toLowerCase() !== "close" && d.AssgToDeptId === depart);
                });
                if ($scope.reporttype == "O") {
                    $scope.modaltasklist1 = $filter('filter')($scope.overallempissuesempissuesview, function (d) {
                        return (d.AssignedToEmpId === empid && d.ISMTCR_Status.toLowerCase() !== "close" && d.AssgToDeptId === depart);
                    });
                    angular.forEach($scope.modaltasklist1, function (emp) {
                        $scope.modaltasklist.push(emp);
                    });
                }
            }
            if ($scope.modaltasklist != null && $scope.modaltasklist.length > 0) {
                $scope.empname = $scope.modaltasklist[0].AssignedTo;
                $scope.empstatus = "Pending";
                $scope.emppriority = "";
                $scope.departview = $scope.Dept.hrmD_DepartmentName;
                $scope.filetrsatuscount();
                $('#myModalPreview').modal('show');
            }
        };

        $scope.emppclose = function (empid, depart, type) {
            $scope.modaltasklist = [];
            $scope.modaltasklist1 = [];
            $scope.currentPage = 1;
            $scope.clearsearch();
            if (type == 1) {
                $scope.overalltype = "Inter-Departmental";
                $scope.modaltasklist = $filter('filter')($scope.departmentempissuesview, function (d) {
                    return (d.AssignedToEmpId === empid && d.ISMTCR_Status.toLowerCase() === "close" && d.AssgToDeptId === depart);
                });
            }
            else if (type == 2) {
                $scope.overalltype = "Department";
                $scope.modaltasklist = $filter('filter')($scope.overallempissuesempissuesview, function (d) {
                    return (d.AssignedToEmpId === empid && d.ISMTCR_Status.toLowerCase() === "close" && d.AssgToDeptId === depart);
                });
            }
            else if (type == 3) {
                $scope.overalltype = "OverAll";
                $scope.modaltasklist = $filter('filter')($scope.departmentempissuesview, function (d) {
                    return (d.AssignedToEmpId === empid && d.ISMTCR_Status.toLowerCase() === "close" && d.AssgToDeptId === depart);
                });
                if ($scope.reporttype == "O") {
                    $scope.modaltasklist1 = $filter('filter')($scope.overallempissuesempissuesview, function (d) {
                        return (d.AssignedToEmpId === empid && d.ISMTCR_Status.toLowerCase() === "close" && d.AssgToDeptId === depart);
                    });
                    angular.forEach($scope.modaltasklist1, function (emp) {
                        $scope.modaltasklist.push(emp);
                    });
                }
            }
            if ($scope.modaltasklist != null && $scope.modaltasklist.length > 0) {
                $scope.empname = $scope.modaltasklist[0].AssignedTo;
                $scope.empstatus = "Close";
                $scope.emppriority = "";
                $scope.departview = $scope.Dept.hrmD_DepartmentName;
                $scope.filetrsatuscount();
                $('#myModalPreview').modal('show');
            }
        };

        $scope.totemppening = function (type, dep, depname) {
            $scope.modaltasklist = [];
            $scope.modaltasklist1 = [];
            $scope.currentPage = 1;
            $scope.empname = "";
            $scope.clearsearch();
            if (type == 1) {
                if (dep == undefined || dep == null) {
                    $scope.overalltype = "Inter-Departmental-" + $scope.Dept.hrmD_DepartmentName;
                    $scope.departview = $scope.Dept.hrmD_DepartmentName;
                    $scope.modaltasklist = $filter('filter')($scope.departmentempissuesview, function (d) {
                        return (d.ISMTCR_Status.toLowerCase() !== "close" && d.AssgToDeptId === $scope.Dept.hrmD_Id);
                    });
                }
                else {
                    $scope.overalltype = "Inter-Departmental-" + depname;
                    $scope.departview = depname;
                    $scope.modaltasklist = $filter('filter')($scope.departmentempissuesviewall, function (d) {
                        return (d.ISMTCR_Status.toLowerCase() !== "close" && d.AssgToDeptId === dep);
                    });
                }
            }
            else if (type == 2) {
                if (dep == undefined || dep == null) {
                    $scope.overalltype = "Department-" + $scope.Dept.hrmD_DepartmentName;
                    $scope.departview = $scope.Dept.hrmD_DepartmentName;
                    $scope.modaltasklist = $filter('filter')($scope.overallempissuesempissuesview, function (d) {
                        return (d.ISMTCR_Status.toLowerCase() !== "close" && d.AssgToDeptId === $scope.Dept.hrmD_Id);
                    });
                }
                else {
                    $scope.overalltype = "Department-" + depname;
                    $scope.departview = depname;
                    $scope.modaltasklist = $filter('filter')($scope.overallempissuesempissuesviewall, function (d) {
                        return (d.ISMTCR_Status.toLowerCase() !== "close" && d.AssgToDeptId === dep);
                    });
                }
            }
            else if (type == 3) {
                if (dep == undefined || dep == null) {
                    $scope.overalltype = "OverAll-" + $scope.Dept.hrmD_DepartmentName;
                    $scope.departview = $scope.Dept.hrmD_DepartmentName;
                    $scope.modaltasklist = $filter('filter')($scope.departmentempissuesview, function (d) {
                        return (d.ISMTCR_Status.toLowerCase() !== "close" && d.AssgToDeptId === $scope.Dept.hrmD_Id);
                    });
                    if ($scope.reporttype == "O") {
                        $scope.modaltasklist1 = $filter('filter')($scope.overallempissuesempissuesview, function (d) {
                            return (d.ISMTCR_Status.toLowerCase() !== "close" && d.AssgToDeptId === $scope.Dept.hrmD_Id);
                        });
                        angular.forEach($scope.modaltasklist1, function (emp) {
                            $scope.modaltasklist.push(emp);
                        });
                    }
                }
                else {
                    $scope.overalltype = "OverAll-" + depname;
                    $scope.departview = depname;
                    $scope.modaltasklist = $filter('filter')($scope.departmentempissuesviewall, function (d) {
                        return (d.ISMTCR_Status.toLowerCase() !== "close" && d.AssgToDeptId === dep);
                    });
                    if ($scope.reporttype == "A") {
                        $scope.modaltasklist1 = $filter('filter')($scope.overallempissuesempissuesviewall, function (d) {
                            return (d.ISMTCR_Status.toLowerCase() !== "close" && d.AssgToDeptId === dep);
                        });
                        angular.forEach($scope.modaltasklist1, function (emp) {
                            $scope.modaltasklist.push(emp);
                        });
                    }
                }
            }
            if ($scope.modaltasklist != null && $scope.modaltasklist.length > 0) {
                $scope.empstatus = "Pending";
                $scope.emppriority = "";
                $scope.filetrsatuscount();
                $('#myModalPreview').modal('show');
            }
        };

        $scope.totemppclose = function (type, dep, depname) {
            $scope.modaltasklist = [];
            $scope.modaltasklist1 = [];
            $scope.currentPage = 1;
            $scope.empname = "";
            $scope.clearsearch();
            if (type == 1) {
                if (dep == undefined || dep == null) {
                    $scope.overalltype = "Inter-Departmental-" + $scope.Dept.hrmD_DepartmentName;
                    $scope.departview = $scope.Dept.hrmD_DepartmentName;
                    $scope.modaltasklist = $filter('filter')($scope.departmentempissuesview, function (d) {
                        return (d.ISMTCR_Status.toLowerCase() === "close" && d.AssgToDeptId === $scope.Dept.hrmD_Id);
                    });
                }
                else {
                    $scope.overalltype = "Inter-Departmental-" + depname;
                    $scope.departview = depname;
                    $scope.modaltasklist = $filter('filter')($scope.departmentempissuesviewall, function (d) {
                        return (d.ISMTCR_Status.toLowerCase() === "close" && d.AssgToDeptId === dep);
                    });
                }
            }
            else if (type == 2) {
                if (dep == undefined || dep == null) {
                    $scope.overalltype = "Department-" + $scope.Dept.hrmD_DepartmentName;
                    $scope.departview = $scope.Dept.hrmD_DepartmentName;
                    $scope.modaltasklist = $filter('filter')($scope.overallempissuesempissuesview, function (d) {
                        return (d.ISMTCR_Status.toLowerCase() === "close" && d.AssgToDeptId === $scope.Dept.hrmD_Id);
                    });
                }
                else {
                    $scope.overalltype = "Department-" + depname;
                    $scope.departview = depname;
                    $scope.modaltasklist = $filter('filter')($scope.overallempissuesempissuesviewall, function (d) {
                        return (d.ISMTCR_Status.toLowerCase() === "close" && d.AssgToDeptId === dep);
                    });
                }
            }
            else if (type == 3) {
                if (dep == undefined || dep == null) {
                    $scope.overalltype = "OverAll-" + $scope.Dept.hrmD_DepartmentName;
                    $scope.departview = $scope.Dept.hrmD_DepartmentName;
                    $scope.modaltasklist = $filter('filter')($scope.departmentempissuesview, function (d) {
                        return (d.ISMTCR_Status.toLowerCase() === "close" && d.AssgToDeptId === $scope.Dept.hrmD_Id);
                    });
                    if ($scope.reporttype == "O") {
                        $scope.modaltasklist1 = $filter('filter')($scope.overallempissuesempissuesview, function (d) {
                            return (d.ISMTCR_Status.toLowerCase() === "close" && d.AssgToDeptId === $scope.Dept.hrmD_Id);
                        });
                        angular.forEach($scope.modaltasklist1, function (emp) {
                            $scope.modaltasklist.push(emp);
                        });
                    }
                }
                else {
                    $scope.overalltype = "OverAll-" + depname;
                    $scope.departview = depname;
                    $scope.modaltasklist = $filter('filter')($scope.departmentempissuesviewall, function (d) {
                        return (d.ISMTCR_Status.toLowerCase() === "close" && d.AssgToDeptId === dep);
                    });
                    if ($scope.reporttype == "A") {
                        $scope.modaltasklist1 = $filter('filter')($scope.overallempissuesempissuesviewall, function (d) {
                            return (d.ISMTCR_Status.toLowerCase() === "close" && d.AssgToDeptId === dep);
                        });
                        angular.forEach($scope.modaltasklist1, function (emp) {
                            $scope.modaltasklist.push(emp);
                        });
                    }
                }
            }
            if ($scope.modaltasklist != null && $scope.modaltasklist.length > 0) {

                $scope.empstatus = "Close";
                $scope.emppriority = "";
                $scope.filetrsatuscount();
                $('#myModalPreview').modal('show');
            }
        };

        $scope.totemptotal = function (type, dep, depname) {
            $scope.modaltasklist = [];
            $scope.modaltasklist1 = [];
            $scope.currentPage = 1;
            $scope.empname = "";
            $scope.clearsearch();
            if (type == 1) {
                if (dep == undefined || dep == null) {
                    $scope.overalltype = "Inter-Departmental-" + $scope.Dept.hrmD_DepartmentName;
                    $scope.departview = $scope.Dept.hrmD_DepartmentName;
                    $scope.modaltasklist = $filter('filter')($scope.departmentempissuesview, function (d) {
                        return (d.AssgToDeptId === $scope.Dept.hrmD_Id);
                    });
                }
                else {
                    $scope.overalltype = "Inter-Departmental-" + depname;
                    $scope.departview = depname;
                    $scope.modaltasklist = $filter('filter')($scope.departmentempissuesviewall, function (d) {
                        return (d.AssgToDeptId === dep);
                    });
                }
            }
            else if (type == 2) {
                if (dep == undefined || dep == null) {
                    $scope.overalltype = "Department-" + $scope.Dept.hrmD_DepartmentName;
                    $scope.departview = $scope.Dept.hrmD_DepartmentName;
                    $scope.modaltasklist = $filter('filter')($scope.overallempissuesempissuesview, function (d) {
                        return (d.AssgToDeptId === $scope.Dept.hrmD_Id);
                    });
                }
                else {
                    $scope.overalltype = "Department-" + depname;
                    $scope.departview = depname;
                    $scope.modaltasklist = $filter('filter')($scope.overallempissuesempissuesviewall, function (d) {
                        return (d.AssgToDeptId === dep);
                    });
                }
            }
            else if (type == 3) {
                if (dep == undefined || dep == null) {
                    $scope.overalltype = "OverAll-" + $scope.Dept.hrmD_DepartmentName;
                    $scope.departview = $scope.Dept.hrmD_DepartmentName;
                    $scope.modaltasklist = $filter('filter')($scope.departmentempissuesview, function (d) {
                        return (d.AssgToDeptId === $scope.Dept.hrmD_Id);
                    });
                    if ($scope.reporttype == "O") {
                        $scope.modaltasklist1 = $filter('filter')($scope.overallempissuesempissuesview, function (d) {
                            return (d.AssgToDeptId === $scope.Dept.hrmD_Id);
                        });
                        angular.forEach($scope.modaltasklist1, function (emp) {
                            $scope.modaltasklist.push(emp);
                        });
                    }
                }
                else {
                    $scope.overalltype = "OverAll-" + depname;
                    $scope.departview = depname;
                    $scope.modaltasklist = $filter('filter')($scope.departmentempissuesviewall, function (d) {
                        return (d.AssgToDeptId === dep);
                    });
                    if ($scope.reporttype == "A") {
                        $scope.modaltasklist1 = $filter('filter')($scope.overallempissuesempissuesviewall, function (d) {
                            return (d.AssgToDeptId === dep);
                        });
                        angular.forEach($scope.modaltasklist1, function (emp) {
                            $scope.modaltasklist.push(emp);
                        });
                    }
                }
            }
            if ($scope.modaltasklist != null && $scope.modaltasklist.length > 0) {

                $scope.empstatus = "Total";
                $scope.emppriority = "";
                $scope.filetrsatuscount();
                $('#myModalPreview').modal('show');
            }
        };

        $scope.departmenttasklist = function (depart, sta, dptname) {
            $scope.modaltasklist = [];
            $scope.departview = "";
            $scope.currentPage = 1;
            $scope.clearsearch();
            if (sta == 'InterMediate') {
                $scope.modaltasklist = $filter('filter')($scope.consolidatedissuesview, function (d) {
                    return (d.ISMTCR_Status.toLowerCase() !== "close" && d.ISMTCR_Status.toLowerCase() !== "open" && d.AssgToDeptId === depart);
                });
            }
            else if (sta == 'Pending') {
                $scope.modaltasklist = $filter('filter')($scope.consolidatedissuesview, function (d) {
                    return (d.ISMTCR_Status.toLowerCase() !== "close" && d.AssgToDeptId === depart);
                });
            }
            else if (sta == 'Total') {
                $scope.modaltasklist = $filter('filter')($scope.consolidatedissuesview, function (d) {
                    return (d.AssgToDeptId === depart);
                });
            }
            else {
                $scope.modaltasklist = $filter('filter')($scope.consolidatedissuesview, function (d) {
                    return (d.ISMTCR_Status.toLowerCase() === sta.toLowerCase() && d.AssgToDeptId === depart);
                });
            }
            if ($scope.modaltasklist != null && $scope.modaltasklist.length > 0) {
                $scope.empname = $scope.modaltasklist[0].AssignedTo;
                $scope.empstatus = sta;
                $scope.departview = dptname;
                $scope.filetrsatuscount();
                $('#myModalPreview').modal('show');
            }
        };

        $scope.interdepartmenttasklist = function (frdepart, sta, frdptname, todeprtid, todprtname) {
            $scope.modaltasklist = [];
            $scope.departview = "";
            $scope.currentPage = 1;
            $scope.clearsearch();
            if (sta == 'InterMediate') {
                $scope.modaltasklist = $filter('filter')($scope.interDepartmentissuesview, function (d) {
                    return (d.ISMTCR_Status.toLowerCase() !== "close" && d.ISMTCR_Status.toLowerCase() !== "open" && d.AssgToDeptId === todeprtid && d.CreatedDeptId === frdepart);
                });
            }
            else if (sta == 'Pending') {
                $scope.modaltasklist = $filter('filter')($scope.interDepartmentissuesview, function (d) {
                    return (d.ISMTCR_Status.toLowerCase() !== "close" && d.AssgToDeptId === todeprtid && d.CreatedDeptId === frdepart);
                });
            }
            else if (sta == 'Total') {
                $scope.modaltasklist = $filter('filter')($scope.interDepartmentissuesview, function (d) {
                    return (d.AssgToDeptId === todeprtid && d.CreatedDeptId === frdepart);
                });
            }
            else {
                $scope.modaltasklist = $filter('filter')($scope.interDepartmentissuesview, function (d) {
                    return (d.ISMTCR_Status.toLowerCase() === sta.toLowerCase() && d.AssgToDeptId === todeprtid && d.CreatedDeptId === frdepart);
                });
            }
            if ($scope.modaltasklist != null && $scope.modaltasklist.length > 0) {
                $scope.empname = $scope.modaltasklist[0].AssignedTo;
                $scope.empstatus = sta;
                $scope.departview = todprtname;
                $scope.todepartview = frdptname;
                $scope.filetrsatuscount();
                $('#myModalPreview').modal('show');
            }
        };

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printmaingrid").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


        $scope.printvocherpage = function () {
            var innerContents = "";
            innerContents = document.getElementById("printvocherpage").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.monthwiseprint = function () {
            var innerContents = "";
            innerContents = document.getElementById("monthwiseprint").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.ledgerwiseprint = function () {
            var innerContents = "";
            innerContents = document.getElementById("ledgerwiseprint").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.totalledgerprint = function () {
            var innerContents = "";
            innerContents = document.getElementById("totalledgerprint").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function () {
            var excelnamemain = "";
            if ($scope.reporttype == "I") {
                excelnamemain = "Inter-Departmental Issue's Count";
            }
            if ($scope.reporttype == "C") {
                excelnamemain = "Inter-Departmental-Consolidated Issue's Count";
            }
            if ($scope.reporttype == "E") {
                excelnamemain = $scope.Dept.hrmD_DepartmentName + "-Employeewise-Inter-Departmental Issues Count";
            }
            if ($scope.reporttype == "O") {
                excelnamemain = $scope.Dept.hrmD_DepartmentName + "-Overall Employeewise Issues Count";
            }
            if ($scope.reporttype == "A") {
                excelnamemain = "Overall All Department Issues Count";
            }
            $scope.from_dateex = $filter('date')($scope.start_Date, 'yyyy-MM-dd');
            $scope.to_dateex = $filter('date')($scope.end_Date, 'yyyy-MM-dd');

            if ($scope.excelnamemain != "") {
                excelnamemain = excelnamemain + '-' + $scope.from_dateex + ' To ' + $scope.to_dateex + '.xls';
            }

            var printSectionId = '#printDeviation';
            var exportHref = Excel.tableToExcel(printSectionId, 'Issues Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelnamemain;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.exportToExcelmodal = function () {
            var excelname = "";
            if ($scope.reporttype == "I") {
                excelname = $scope.todepartview + ' On ' + $scope.departview;
            }
            if ($scope.reporttype == "C") {
                excelname = $scope.departview;
            }
            if ($scope.reporttype == "E") {
                if ($scope.empname != null && $scope.empname != "") {
                    excelname = $scope.empname + '-Inter Department';
                }
                else {
                    excelname = $scope.Dept.hrmD_DepartmentName + '-Inter Department';
                }
            }
            if ($scope.reporttype == "O") {
                if ($scope.empname != null && $scope.empname != "") {
                    excelname = $scope.empname + '-OverAll Issues-' + $scope.overalltype;
                }
                else {
                    excelname = 'OverAll Issues-' + $scope.overalltype;
                }
            }
            if ($scope.reporttype == "A") {
                excelname = 'OverAll Issues-' + $scope.overalltype;
            }
            if ($scope.empstatus != "") {
                excelname = excelname + '-' + $scope.empstatus;
            }
            if ($scope.emppriority != "" && $scope.emppriority != undefined) {
                excelname = excelname + '-' + $scope.emppriority;
            }
            $scope.from_dateex = $filter('date')($scope.start_Date, 'yyyy-MM-dd');
            $scope.to_dateex = $filter('date')($scope.end_Date, 'yyyy-MM-dd');
            if ($scope.searchValuecol != undefined) {
                if (($scope.searchValuecol.ISMTCR_Status != undefined && $scope.searchValuecol.ISMTCR_Status != null && $scope.searchValuecol.ISMTCR_Status != '') || ($scope.searchValuecol.Priority != undefined && $scope.searchValuecol.Priority != null && $scope.searchValuecol.Priority != '') || ($scope.searchValuecol.Createdby != undefined && $scope.searchValuecol.Createdby != null && $scope.searchValuecol.Createdby != '') || ($scope.searchValuecol.AssignedBy != undefined && $scope.searchValuecol.AssignedBy != null && $scope.searchValuecol.AssignedBy != '') || ($scope.searchValuecol.AssignedTo != undefined && $scope.searchValuecol.AssignedTo != null && $scope.searchValuecol.AssignedTo != '') || ($scope.searchValueall != undefined && $scope.searchValueall != null && $scope.searchValueall != '')) {
                    excelname = excelname + '[Filtered]';
                }
            }
            if (excelname != "") {
                excelname = excelname + '-' + $scope.from_dateex + ' To ' + $scope.to_dateex + '.xls';
            }
            var printSectionId = '#table1';
            var exportHref = Excel.tableToExcel(printSectionId, 'Issues Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.Clear = function () {
            //$scope.consolidatedissues = "";
            //$scope.interdeprtissuefilter = "";
            //$scope.interDepartmentissues = "";
            //$scope.departmentempissues = "";
            //$scope.overallempissues = "";
            //$scope.start_Date = "";
            //$scope.end_Date = "";
            //$scope.submitted = false;
            //$scope.emparrayexist = false;
            //$scope.consolidatedissuesview = [];
            //$scope.interDepartmentissuesview = [];
            //$scope.departmentempissuesview = [];
            //$scope.overallempissuesempissuesview = [];
            //$scope.ovrallissuecountempwise = [];
            //$scope.modaltasklist = [];
            $state.reload();
        };

        $scope.Clearondate = function () {
            $scope.consolidatedissues = "";
            $scope.interdeprtissuefilter = "";
            $scope.interDepartmentissues = "";
            $scope.departmentempissues = "";
            $scope.departmentempissuesall = "";
            $scope.overallempissuesall = "";
            $scope.overallempissues = "";
            $scope.submitted = false;
            $scope.emparrayexist = false;
            $scope.consolidatedissuesview = [];
            $scope.interDepartmentissuesview = [];
            $scope.departmentempissuesview = [];
            $scope.overallempissuesempissuesview = [];
            $scope.ovrallissuecountempwise = [];
            $scope.ovrstatuswiseemployees = [];
            $scope.modaltasklist = [];
            //$state.reload();
        };

        $scope.clearonchange = function () {
            $scope.departmentempissues = [];
            //$scope.departmentempissuesall = [];
            //$scope.overallempissuesall = [];
            $scope.overallempissues = [];
            $scope.statusfilteredlist = [];
            $scope.priorityfilteredlist = [];
            $scope.empfilteredlist = [];
            $scope.statuswiseemployees = [];
            $scope.departmentempissuesview = [];
            $scope.overallempissuesempissuesview = [];
            $scope.ovrallissuecountempwise = [];
            $scope.ovrstatuswiseemployees = [];
            $scope.modaltasklist = [];
            $scope.emparrayexist = false;
            //$state.reload();
        };

        $scope.clearsearch = function () {
            $scope.exact = false;
            if ($scope.searchValuecol != undefined) {
                if ($scope.searchValuecol.ISMTCR_Status != undefined) {
                    $scope.searchValuecol.ISMTCR_Status = "";
                }
                if ($scope.searchValuecol.Priority != undefined) {
                    $scope.searchValuecol.Priority = "";
                }
                if ($scope.searchValuecol.Createdby != undefined) {
                    $scope.searchValuecol.Createdby = "";
                }
                if ($scope.searchValuecol.AssignedBy != undefined) {
                    $scope.searchValuecol.AssignedBy = "";
                }
                if ($scope.searchValuecol.AssignedTo != undefined) {
                    $scope.searchValuecol.AssignedTo = "";
                }
            }
            if ($scope.searchValueall != undefined) {
                $scope.searchValueall = "";
            }
        };

        $scope.filetrsatuscount = function () {
            $scope.modalstatuslist = [];
            $scope.modalstatuslisttemp = [];
            $scope.modalstatuslisttemp = $scope.modaltasklist.filter((item, i, arr) => arr.findIndex((t) => t.ISMTCR_Status.toLowerCase() === item.ISMTCR_Status.toLowerCase()) === i);
            angular.forEach($scope.modalstatuslisttemp, function (emp) {
                $scope.modalstatuslist.push({ status: emp.ISMTCR_Status });
            });
        };

        $scope.viewresponsemultiple = function (taskobj, fileview, taskindmulti, pagenomulti, f) {
            taskobj = $filter('filter')($scope.filterValue, function (d) {
                return (d.ISMTCR_Id === taskobj.ISMTCR_Id);
            });
            $scope.submitted1 = false;
            pagenomulti = 1;
            $scope.taskindexmulti = taskindmulti;
            $scope.pagetaskmulti = 0;
            if (pagenomulti > 1) {
                $scope.pagetaskmulti = 10 * (pagenomulti - 1);
            }
            $scope.taskindexmulti = $scope.taskindexmulti + $scope.pagetaskmulti;
            $scope.currenttaskslengthmulti = ($scope.taskindexmulti + 1);
            if ($scope.taskindexmulti == 0) {
                $scope.backtaskshowmulti = false;
            }
            else {
                $scope.backtaskshowmulti = true;
            }
            $scope.statusNamemodal = "";
            $scope.tasktitle = "";
            $scope.taskno = "";
            $scope.tasktype = "";
            $scope.tsakpriority = "";
            $scope.priorityswitch = "";
            $scope.prioritypreswitch = "";
            $scope.created_by = "";
            $scope.plancount = "";
            $scope.periodicity = "";
            $scope.Rep_assignedto = "";
            $scope.rep_assignedby = "";
            $scope.rep_assignedDate = "";
            $scope.leftflag = "";
            $scope.Rep_module = "";
            $scope.taskdate = "";
            $scope.taskstatus = "";
            $scope.startdate = "";
            $scope.enddate = "";
            $scope.effhrstemp = "";
            $scope.effmintemp = "";
            $scope.taskdescription = "";
            $scope.ismtcrreS_Response = "";
            $scope.files_paths = "";
            $scope.ismmistS_StatusName = "";
            $scope.taskAttatchment = [];
            $scope.get_taskdetails = [];
            $scope.objupdate.eachTaskMaxDuration = 0;
            $scope.objupdate.durationFlg = "";
            $scope.get_responseattachments = [];
            $scope.taskidmulti = taskobj[0].ISMTCR_Id;
            var data = {
                "ISMTCR_Id": $scope.taskidmulti
            };
            apiService.create("ISM_ComplaintsTaskList/gettaskdetails", data).
                then(function (promise) {
                    $scope.get_taskdetails = promise.get_taskdetails;
                    $scope.get_taskresponse = promise.get_taskresponse;
                    if (promise.role_flag == "Head") {
                        $scope.headornot = true
                    }
                    else {
                        $scope.headornot = false
                    }
                    if ($scope.get_taskdetails !== null && $scope.get_taskdetails.length > 0) {

                        $scope.userhrmeid = promise.hrmE_Id;
                        $scope.createdhrmeid = $scope.get_taskdetails[0].createdhrmeid;
                        $scope.crdprthead = $scope.get_taskdetails[0].crdprthead;
                        $scope.tasktitle = $scope.get_taskdetails[0].ISMTCR_Title;
                        $scope.taskno = $scope.get_taskdetails[0].ISMTCR_TaskNo;
                        $scope.taskclient = $scope.get_taskdetails[0].ISMMCLT_ClientName;
                        $scope.Rep_module = $scope.get_taskdetails[0].IVRMM_ModuleName;
                        $scope.taskdate = $scope.get_taskdetails[0].ISMTCR_CreationDate;
                        $scope.tasktype = $scope.get_taskdetails[0].tasktype;
                        $scope.tsakpriority = $scope.get_taskdetails[0].HRMP_Name;
                        $scope.priorityswitch = $scope.get_taskdetails[0].priorityswitch;
                        $scope.prioritypreswitch = $scope.get_taskdetails[0].preswitch;
                        $scope.taskdescription = $scope.get_taskdetails[0].ISMTCR_Desc;
                        $scope.taskstatus = $scope.get_taskdetails[0].ISMTCR_Status;
                        $scope.createdby = $scope.get_taskdetails[0].createdby;
                        $scope.Rep_assignedto = $scope.get_taskdetails[0].assignedto;
                        $scope.rep_assignedby = $scope.get_taskdetails[0].assignedby;
                        $scope.rep_assignedDate = $scope.get_taskdetails[0].assignedDate;
                        $scope.leftflag = $scope.get_taskdetails[0].empleft;
                        $scope.plancount = $scope.get_taskdetails[0].plancount;
                        $scope.periodicity = $scope.get_taskdetails[0].ISMTAPL_Periodicity;

                        $scope.Responsemodal = taskobj[0].ismtcrreS_Response;
                        $scope.created_by = $scope.get_taskdetails[0].createdby;
                        if ($scope.get_taskdetails[0].ISMTCRASTO_StartDate != null) {
                            $scope.startdate = new Date($scope.get_taskdetails[0].ISMTCRASTO_StartDate).toDateString();
                            $scope.enddate = new Date($scope.get_taskdetails[0].ISMTCRASTO_EndDate).toDateString();
                        }
                        $scope.effort = $scope.get_taskdetails[0].ISMTCRASTO_EffortInHrs;
                        $scope.objupdate.eachTaskMaxDuration = $scope.get_taskdetails[0].ISMTCRASTO_EffortInHrs;
                        $scope.objupdate.durationFlg = "HOURS";
                        var decimaltimetot = $scope.effort;
                        var hrstot = parseInt(Number(decimaltimetot));
                        var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);
                        $scope.objupdate.effhrs = hrstot;
                        $scope.objupdate.effmin = mintot;
                        if (hrstot.toString().length === 1) {
                            hrstot = '0' + hrstot;
                        }
                        if (mintot.toString().length === 1) {
                            mintot = '0' + mintot;
                        }
                        $scope.effhrstemp = hrstot;
                        $scope.effmintemp = mintot;
                        $scope.statusNamemodal = taskobj[0].ismmistS_StatusName;

                        $scope.taskAttatchment = promise.get_taskattachments;
                        $scope.get_responseattachments = promise.get_responseattachments;
                        if ($scope.taskAttatchment.length > 0) {
                            angular.forEach($scope.taskAttatchment, function (at) {
                                if (at.ISMTCRAT_Attatchment !== null && at.ISMTCRAT_Attatchment !== "") {
                                    var img = at.ISMTCRAT_Attatchment;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];
                                    at.filetype = lastelement;
                                }
                            });
                        }
                        $scope.respattachement = false;
                        if ($scope.get_responseattachments.length > 0) {
                            angular.forEach($scope.get_responseattachments, function (at) {
                                if (at.ISMTCRRES_ResponseAttachment !== null && at.ISMTCRRES_ResponseAttachment !== "") {
                                    var img1 = at.ISMTCRRES_ResponseAttachment;
                                    var imagarr1 = img1.split('.');
                                    var lastelement1 = imagarr1[imagarr1.length - 1];
                                    at.filetype = lastelement1;
                                    $scope.respattachement = true;
                                }
                            });
                        }
                        if ($scope.get_taskresponse.length > 0) {
                            if ($scope.get_responseattachments.length > 0) {
                                angular.forEach($scope.get_taskresponse, function (at) {
                                    var slno = 0;
                                    angular.forEach($scope.get_responseattachments, function (atr) {
                                        if (at.ISMTCRRES_Response === atr.ISMTCRRES_Response) {
                                            slno = slno + 1;
                                            atr.slno = slno;
                                        }
                                    });
                                    //var lines = 1;
                                    //lines = at.ISMTCRRES_Response.split(/\r\n|\r|\n/).length;
                                    //at.lines = lines;
                                    at.student_rmrks = at.ISMTCRRES_Response.split("\n");
                                });
                            }
                        }
                        $('#myModalPreview').modal('hide');
                        $('#myModalResponse').modal('show');
                    }
                    else {
                        $('#myModalResponse').modal('hide');
                    }
                });
        };

        $scope.nexttaskmulti = function () {
            $scope.taskindexmulti = $scope.taskindexmulti + 1;
            $scope.nexttaskobjm.ISMTCR_Id = $scope.filterValue[$scope.taskindexmulti].ISMTCR_Id;
            $scope.nexttaskobjm.assignedto = $scope.filterValue[$scope.taskindexmulti].assignedto;
            $scope.nexttaskobjm.checkedvalue = $scope.filterValue[$scope.taskindexmulti].checkedvalue;
            $scope.viewresponsemultiple($scope.nexttaskobjm, '', $scope.taskindexmulti);
        };

        $scope.backtaskmulti = function () {
            $scope.taskindexmulti = $scope.taskindexmulti - 1;
            $scope.nexttaskobjm.ISMTCR_Id = $scope.filterValue[$scope.taskindexmulti].ISMTCR_Id;
            $scope.nexttaskobjm.assignedto = $scope.filterValue[$scope.taskindexmulti].assignedto;
            $scope.nexttaskobjm.checkedvalue = $scope.filterValue[$scope.taskindexmulti].checkedvalue;
            $scope.viewresponsemultiple($scope.nexttaskobjm, '', $scope.taskindexmulti);
        };

        $scope.closeresponse = function () {
            $('#myModalPreview').modal('show');
            $('#myModalResponse').modal('hide');
        };

        $scope.closeview = function () {
            $('#myModalPreview').modal('hide');
            $('#showpdf').modal('hide');
            $('#myModaldocx').modal('hide');
            $('#myvideoPreview').modal('hide');
            $('#myaudioPreview').modal('hide');
            $('#myModalResponse').modal('show');
        };

        //=========== View Files
        $scope.previewimg = function (data, ind, typ) {
            $scope.docindex = ind;
            $scope.showtype = typ;
            $('#showpdf').modal('hide');
            $('#myModal2').modal('hide');
            $('#myModaldocx').modal('hide');
            $('#myvideoPreview').modal('hide');
            $('#myaudioPreview').modal('hide');
            $scope.attachmentsshow = [];
            var docpath = "";
            if (typ == 1) {
                $scope.attachmentsshow = $scope.taskAttatchment;
            }
            else if (typ == 2) {
                $scope.attachmentsshow = $scope.get_responseattachments;
            }
            if (data === undefined || data === null) {
                if ($scope.docindex > $scope.attachmentsshow.length - 1) {
                    $scope.docindex = 0;
                }
                if (typ == 1) {
                    docpath = $scope.attachmentsshow[$scope.docindex].ISMTCRAT_Attatchment;
                }
                else if (typ == 2) {
                    docpath = $scope.attachmentsshow[$scope.docindex].ISMTCRRES_ResponseAttachment;
                }
                var namedoc = "";
                if (docpath != null && docpath != undefined) {
                    $scope.docname = namedoc;
                    $scope.docnameno = $scope.docindex + 1;
                    $('#preview').attr('src', docpath);
                    $('#myModalResponse').modal('hide');

                    $('.modal-backdrop').remove();
                    $('#myModalPreviewimg').modal('show');
                    $('.modal').on('hide.bs.modal', function (e) {
                        e.stopPropagation();
                        $('body').css('padding-right', '');
                    });
                }
            } else {
                if (typ == 1) {
                    $scope.docname = data.ISMTCRAT_Attatchment;
                }
                else if (typ == 2) {
                    $scope.docname = data.ISMTCRRES_ResponseAttachment;
                    angular.forEach($scope.attachmentsshow, function (pri) {
                        if (pri.ISMTCRRES_Id === data.ISMTCRRES_Id) {
                            $scope.docname = pri.ISMTCRRES_ResponseAttachment;
                        }
                    });
                }
                $scope.docnameno = $scope.docindex + 1;
                $('#preview').attr('src', $scope.docname);
                $('#myModalResponse').modal('hide');

                $('.modal-backdrop').remove();
                $('#myModalPreviewimg').modal('show');
                $('.modal').on('hide.bs.modal', function (e) {
                    e.stopPropagation();
                    $('body').css('padding-right', '');
                });
            }
            if ($scope.docindex <= $scope.attachmentsshow.length - 1) {
                $scope.shownext = true;
            }
            else {
                $scope.shownext = false;
            }
            if (typ == 2) {
                $scope.shownext = false;
            }
        };

        $scope.downloadpdf = function (data, ind, typ) {
            $scope.docindex = ind;
            $scope.showtype = typ;
            $('#myModalResponse').modal('hide');
            $('#myModalPreviewimg').modal('hide');
            $('#myModaldocx').modal('hide');
            $('#myvideoPreview').modal('hide');
            $('#myaudioPreview').modal('hide');
            var imagedownload1 = "";
            $scope.attachmentsshow = [];
            var docpath = "";
            if (typ == 1) {
                $scope.attachmentsshow = $scope.taskAttatchment;
            }
            else if (typ == 2) {
                $scope.attachmentsshow = $scope.get_responseattachments;
            }

            if (data === undefined || data === null) {
                if ($scope.docindex > $scope.attachmentsshow.length - 1) {
                    $scope.docindex = 0;
                }
            }
            if (typ == 1) {
                docpath = $scope.attachmentsshow[$scope.docindex].ISMTCRAT_Attatchment;
            }
            else if (typ == 2) {
                angular.forEach($scope.attachmentsshow, function (pri) {
                    if (pri.ISMTCRRES_Id === data.ISMTCRRES_Id) {
                        docpath = pri.ISMTCRRES_ResponseAttachment;
                    }
                });
            }
            var namedoc = "";
            if (docpath != null && docpath != undefined) {
                $scope.docname = namedoc;
                $scope.docnameno = $scope.docindex + 1;
                imagedownload1 = docpath;
            }
            $http.get(imagedownload1, { responseType: 'arraybuffer' })
                .success(function (response) {
                    var fileURL = "";
                    var file = "";
                    var embed = "";
                    var pdfId = "";
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);

                    pdfId = document.getElementById("pdfIdzz");
                    pdfId.removeChild(pdfId.childNodes[0]);
                    embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '1000');
                    pdfId.appendChild(embed);
                    $('#showpdf').modal('show');
                    $('.modal').on('hide.bs.modal', function (e) {
                        e.stopPropagation();
                        $('body').css('padding-right', '');
                    });
                });

            if ($scope.docindex <= $scope.attachmentsshow.length - 1) {
                $scope.shownext = true;
            }
            else {
                $scope.shownext = false;
            }
            if (typ == 2) {
                $scope.shownext = false;
            }
        }

        $scope.viewdox = function (data, ind, typ) {
            $scope.docindex = ind;
            $scope.showtype = typ;
            $('#myModalResponse').modal('hide');
            $('#myModalPreviewimg').modal('hide');
            $('#showpdf').modal('hide')
            $('#myvideoPreview').modal('hide');
            $('#myaudioPreview').modal('hide');
            var imagedownload1 = "";
            $scope.attachmentsshow = [];
            var docpath = "";
            if (typ == 1) {
                $scope.attachmentsshow = $scope.taskAttatchment;
            }
            else if (typ == 2) {
                $scope.attachmentsshow = $scope.get_responseattachments;
            }
            if (data === undefined || data === null) {
                if ($scope.docindex > $scope.attachmentsshow.length - 1) {
                    $scope.docindex = 0;
                }
            }
            if (typ == 1) {
                docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.attachmentsshow[$scope.docindex].ISMTCRAT_Attatchment;
            }
            else if (typ == 2) {

                angular.forEach($scope.attachmentsshow, function (pri) {
                    if (pri.ISMTCRRES_Id === data.ISMTCRRES_Id) {
                        docpath = pri.ISMTCRRES_ResponseAttachment;
                    }
                });
                docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + docpath;
            }
            var namedoc = "";
            if (docpath != null && docpath != undefined) {
                $scope.docname = namedoc;
                $scope.docnameno = $scope.docindex + 1;
                $scope.imagedownload1 = docpath;
            }
            $scope.detailFrame = $sce.trustAsResourceUrl(docpath);
            $('#myModaldocx').modal('show');
            $('.modal').on('hide.bs.modal', function (e) {
                e.stopPropagation();
                $('body').css('padding-right', '');
            });
            if ($scope.docindex <= $scope.attachmentsshow.length - 1) {
                $scope.shownext = true;
            }
            else {
                $scope.shownext = false;
            }
            if (typ == 2) {
                $scope.shownext = false;
            }
        }

        $scope.viewvideo = function (data, ind, typ) {
            $scope.docindex = ind;
            $scope.showtype = typ;
            $('#myModalResponse').modal('hide');
            $('#myModalPreviewimg').modal('hide');
            $('#showpdf').modal('hide');
            $('#myModaldocx').modal('hide');
            $('#myaudioPreview').modal('hide');
            $scope.view_videos = [];
            $scope.attachmentsshow = [];
            var docpath = "";
            if (typ == 1) {
                $scope.attachmentsshow = $scope.taskAttatchment;
            }
            else if (typ == 2) {
                $scope.attachmentsshow = $scope.get_responseattachments;
            }
            if (data === undefined || data === null) {
                if ($scope.docindex > $scope.attachmentsshow.length - 1) {
                    $scope.docindex = 0;
                }
            }
            if (typ == 1) {
                docpath = $scope.attachmentsshow[$scope.docindex].ISMTCRAT_Attatchment;
            }
            else if (typ == 2) {
                angular.forEach($scope.attachmentsshow, function (pri) {
                    if (pri.ISMTCRRES_Id === data.ISMTCRRES_Id) {
                        docpath = pri.ISMTCRRES_ResponseAttachment;
                    }
                });
            }
            var namedoc = "";
            if (docpath != null && docpath != undefined) {
                $scope.docname = namedoc;
                $scope.docnameno = $scope.docindex + 1;
                $scope.urlvideo = docpath;
            }
            $scope.view_videos.push({ id: 1, ihw_video: docpath });
            $('#myvideoPreview').modal('show');
            $('.modal').on('hide.bs.modal', function (e) {
                e.stopPropagation();
                $('body').css('padding-right', '');
            });
            if ($scope.docindex <= $scope.attachmentsshow.length - 1) {
                $scope.shownext = true;
            }
            else {
                $scope.shownext = false;
            }
            if (typ == 2) {
                $scope.shownext = false;
            }
        }

        $scope.viewaudio = function (data, ind, typ) {
            $scope.docindex = ind;
            $scope.showtype = typ;
            $('#myModalResponse').modal('hide');
            $('#myModalPreviewimg').modal('hide');
            $('#showpdf').modal('hide');
            $('#myModaldocx').modal('hide');
            $('#myvideoPreview').modal('hide');
            $scope.view_videos = [];
            var imagedownload1 = "";
            $scope.attachmentsshow = [];
            var docpath = "";
            if (typ == 1) {
                $scope.attachmentsshow = $scope.taskAttatchment;
            }
            else if (typ == 2) {
                $scope.attachmentsshow = $scope.get_responseattachments;

            }
            if (data === undefined || data === null) {
                if ($scope.docindex > $scope.attachmentsshow.length - 1) {
                    $scope.docindex = 0;
                }
            }
            if (typ == 1) {
                docpath = $scope.attachmentsshow[$scope.docindex].ISMTCRAT_Attatchment;
            }
            else if (typ == 2) {
                angular.forEach($scope.attachmentsshow, function (pri) {
                    if (pri.ISMTCRRES_Id === data.ISMTCRRES_Id) {
                        docpath = pri.ISMTCRRES_ResponseAttachment;
                    }
                });
            }
            var namedoc = "";
            if (docpath != null && docpath != undefined) {
                $scope.docname = namedoc;
                $scope.docnameno = $scope.docindex + 1;
                $scope.urlvideo = docpath;
            }
            $scope.view_videos.push({ id: 1, ihw_video: docpath });
            $('#myaudioPreview').modal('show');
            $('.modal').on('hide.bs.modal', function (e) {
                e.stopPropagation();
                $('body').css('padding-right', '');
            });
            if ($scope.docindex <= $scope.attachmentsshow.length - 1) {
                $scope.shownext = true;
            }
            else {
                $scope.shownext = false;
            }
            if (typ == 2) {
                $scope.shownext = false;
            }
        }

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };

        $scope.SendEmail = function () {
            var Template = document.getElementById("SendEmail").innerHTML;
            var data = {



                "Emailcontent": Template

            };
            apiService.create("ISM_InterdepartmentIsuuesReport/SendEmail", data).then(function (promise) {
                if (promise.result == "success") {
                    swal("Email Sent Successfully");
                } else {
                    swal("Request Failed");
                }

            });
        }
    }

    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });
})();

