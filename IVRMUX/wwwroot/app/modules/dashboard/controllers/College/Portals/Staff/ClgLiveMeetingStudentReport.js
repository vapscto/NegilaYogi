(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgLiveMeetingStudentReportController', ClgLiveMeetingStudentReportController);

    ClgLiveMeetingStudentReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache', 'Excel','$timeout'];
    function ClgLiveMeetingStudentReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache, Excel, $timeout) {
        //object

        //Employeee
        $scope.currentPage = 1;
        $scope.searchValue = '';
        $scope.itemsPerPage = 10;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getDATA("ClgLiveMeetingSchedule/getstudentprofiledata").then(function (promise) {

                $scope.employeedropdown = promise.fillstudentalldetails;
                $scope.yearlt = promise.academicList;
                

            });
        };

        // retun details
        $scope.employeeDetails = [];
        $scope.maindata = [];


        $scope.currentemployeequalificationDetails = [];
        $scope.employequalify = false;
        $scope.employeedocumentflg = false;
        $scope.employeeclasssubjectflg = false;
        $scope.qualifymsg = false;
        $scope.empdetails = {};
        $scope.empqualifydetails = {};
        //Search employee
        $scope.submitted = false;

  

        $scope.cleardata = function () {
            $state.reload();
        };

        $scope.OnEmployeeChange = function () {
            $scope.qualifymsg = false;
        };

        $scope.all_check_empl = function (empl) {
            var toggleStatus4 = empl;
            angular.forEach($scope.employeedropdown, function (itm) {
                itm.emple = toggleStatus4;
            });
        };



        $scope.togchkbx = function () {
            debugger;
            $scope.empl = $scope.employeedropdown.every(function (options) {
                return options.emple;
            });
        }

        $scope.searchDiv = true;
        $scope.empDiv = false;


        //Clear data
     
     

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        
        $scope.employeeTypeselected = [];
        $scope.SearchEmployee = function () {

            $scope.submitted = true;
            debugger;
            if ($scope.myForm.$valid) {


               

            $scope.employeeTypeselected = [];
                angular.forEach($scope.employeedropdown, function (itm) {
                    if (itm.emple) {
                        $scope.employeeTypeselected.push({ amst_Id: itm.amst_Id, name: itm.amst_FirstName});
                }

            });
            

                var fromdate1 = $scope.FromDate == null ? "" : $filter('date')($scope.FromDate, "yyyy-MM-dd");
                var todate1 = $scope.ToDate == null ? "" : $filter('date')($scope.ToDate, "yyyy-MM-dd");

            var data = {
                "FromDate": fromdate1,
                "ASMAY_Id": $scope.asmaY_Id,
                "ToDate": todate1,
                "rtype": $scope.type,
                stids: $scope.employeeTypeselected

            };

                apiService.create("ClgLiveMeetingSchedule/getstudentprofilereport", data).
                then(function (promise) {
                    $scope.staffdata = $scope.employeeTypeselected;
                    if ( (promise.meetingliststaff !== null && promise.meetingliststaff.length > 0)) {
                       
                        $scope.meetingliststaff = promise.meetingliststaff;
                        if ($scope.type=='SCH') {

                            //distinct meeting id
                            $scope.meetingidlist = [];
                          
                            

                            if ($scope.meetingliststaff.length>0) {
                                angular.forEach($scope.meetingliststaff, function (eps) {
                                    if ($scope.meetingidlist.length == 0) {
                                        $scope.meetingidlist.push({
                                            LMSLMEET_Id: eps.LMSLMEET_Id, LMSLMEET_MeetingId: eps.LMSLMEET_MeetingId, LMSLMEET_PlannedDate: eps.LMSLMEET_PlannedDate, LMSLMEET_MeetingTopic: eps.LMSLMEET_MeetingTopic, LMSLMEET_PlannedStartTime: eps.LMSLMEET_PlannedStartTime, LMSLMEET_PlannedEndTime: eps.LMSLMEET_PlannedEndTime, LMSLMEET_MeetingDate: eps.LMSLMEET_MeetingDate, LMSLMEET_StartedTime: eps.LMSLMEET_StartedTime, LMSLMEET_EndTime: eps.LMSLMEET_EndTime
                                        });
                                    }
                                    else if ($scope.meetingidlist.length > 0) {
                                        var al_exm_cnt = 0;
                                        angular.forEach($scope.meetingidlist, function (exm) {
                                            if (exm.LMSLMEET_Id == eps.LMSLMEET_Id) {
                                                al_exm_cnt += 1;
                                            }
                                        })
                                        if (al_exm_cnt == 0) {
                                            $scope.meetingidlist.push({
                                                LMSLMEET_Id: eps.LMSLMEET_Id, LMSLMEET_MeetingId: eps.LMSLMEET_MeetingId, LMSLMEET_PlannedDate: eps.LMSLMEET_PlannedDate, LMSLMEET_MeetingTopic: eps.LMSLMEET_MeetingTopic, LMSLMEET_PlannedStartTime: eps.LMSLMEET_PlannedStartTime, LMSLMEET_PlannedEndTime: eps.LMSLMEET_PlannedEndTime, LMSLMEET_MeetingDate: eps.LMSLMEET_MeetingDate, LMSLMEET_StartedTime: eps.LMSLMEET_StartedTime, LMSLMEET_EndTime: eps.LMSLMEET_EndTime
                                            });
                                        }
                                    }

                                })
                            }
                            

                         

                            var meetingdata = [];
                            $scope.maindata = [];
                            angular.forEach($scope.staffdata, function (ss) {


                                angular.forEach($scope.meetingidlist, function (mm) {

                                    angular.forEach($scope.meetingliststaff, function (cc) {

                                        if (ss.amst_Id == cc.AMST_Id && mm.LMSLMEET_Id == cc.LMSLMEET_Id) {
                                            $scope.maindata.push({ LMSLMEET_Id: cc.LMSLMEET_Id, LMSLMEET_MeetingId: cc.LMSLMEET_MeetingId, LMSLMEET_PlannedDate: cc.LMSLMEET_PlannedDate, LMSLMEET_MeetingTopic: cc.LMSLMEET_MeetingTopic, LMSLMEET_PlannedStartTime: cc.LMSLMEET_PlannedStartTime, LMSLMEET_PlannedEndTime: cc.LMSLMEET_PlannedEndTime, LMSLMEET_MeetingDate: cc.LMSLMEET_MeetingDate, LMSLMEET_StartedTime: cc.LMSLMEET_StartedTime, LMSLMEET_EndTime: cc.LMSLMEET_EndTime, ATT: cc.EmpName, AMCO_CourseName: cc.AMCO_CourseName, AMB_BranchName: cc.AMB_BranchName, AMSE_SEMName: cc.AMSE_SEMName, ACMS_SectionName: cc.ACMS_SectionName, sname: ss.name, ISMS_SubjectName: cc.ISMS_SubjectName })
                                        }

                                    })


                                })


                            })




                            console.log($scope.maindata)


                        }
                        else {

                            //distinct meeting id
                            $scope.meetingidlist = [];
                            if ($scope.meetingliststaff.length > 0) {
                                angular.forEach($scope.meetingliststaff, function (eps) {
                                    if ($scope.meetingidlist.length == 0) {
                                        $scope.meetingidlist.push({
                                            LMSLMEET_Id: eps.LMSLMEET_Id, LMSLMEET_MeetingId: eps.LMSLMEET_MeetingId, LMSLMEET_PlannedDate: eps.LMSLMEET_PlannedDate, LMSLMEET_MeetingTopic: eps.LMSLMEET_MeetingTopic, LMSLMEET_PlannedStartTime: eps.LMSLMEET_PlannedStartTime, LMSLMEET_PlannedEndTime: eps.LMSLMEET_PlannedEndTime, LMSLMEET_MeetingDate: eps.LMSLMEET_MeetingDate, LMSLMEET_StartedTime: eps.LMSLMEET_StartedTime, LMSLMEET_EndTime: eps.LMSLMEET_EndTime
                                        });
                                    }
                                    else if ($scope.meetingidlist.length > 0) {
                                        var al_exm_cnt = 0;
                                        angular.forEach($scope.meetingidlist, function (exm) {
                                            if (exm.LMSLMEET_Id == eps.LMSLMEET_Id) {
                                                al_exm_cnt += 1;
                                            }
                                        })
                                        if (al_exm_cnt == 0) {
                                            $scope.meetingidlist.push({
                                                LMSLMEET_Id: eps.LMSLMEET_Id, LMSLMEET_MeetingId: eps.LMSLMEET_MeetingId, LMSLMEET_PlannedDate: eps.LMSLMEET_PlannedDate, LMSLMEET_MeetingTopic: eps.LMSLMEET_MeetingTopic, LMSLMEET_PlannedStartTime: eps.LMSLMEET_PlannedStartTime, LMSLMEET_PlannedEndTime: eps.LMSLMEET_PlannedEndTime, LMSLMEET_MeetingDate: eps.LMSLMEET_MeetingDate, LMSLMEET_StartedTime: eps.LMSLMEET_StartedTime, LMSLMEET_EndTime: eps.LMSLMEET_EndTime
                                            });
                                        }
                                    }

                                })
                            }
                            
                            var meetingdata = [];
                            $scope.maindata = [];
                            angular.forEach($scope.staffdata, function (ss) {


                                angular.forEach($scope.meetingidlist, function (mm) {
                                   
                                    angular.forEach($scope.meetingliststaff, function (cc) {

                                        if (ss.amst_Id == cc.AMST_Id && mm.LMSLMEET_Id == cc.LMSLMEET_Id) {
                                            $scope.maindata.push({ LMSLMEET_Id: cc.LMSLMEET_Id, LMSLMEET_MeetingId: cc.LMSLMEET_MeetingId, LMSLMEET_PlannedDate: cc.LMSLMEET_PlannedDate, LMSLMEET_MeetingTopic: cc.LMSLMEET_MeetingTopic, LMSLMEET_PlannedStartTime: cc.LMSLMEET_PlannedStartTime, LMSLMEET_PlannedEndTime: cc.LMSLMEET_PlannedEndTime, LMSLMEET_MeetingDate: cc.LMSLMEET_MeetingDate, LMSLMEET_StartedTime: cc.LMSLMEET_StartedTime, LMSLMEET_EndTime: cc.LMSLMEET_EndTime, ATT: cc.EmpName, AMCO_CourseName: cc.AMCO_CourseName, AMB_BranchName: cc.AMB_BranchName, AMSE_SEMName: cc.AMSE_SEMName, ACMS_SectionName: cc.ACMS_SectionName, sname: ss.name, ISMS_SubjectName: cc.ISMS_SubjectName, LMSLMEETSTD_LoginTime: cc.LMSLMEETSTD_LoginTime, LMSLMEETSTD_LogoutTime: cc.LMSLMEETSTD_LogoutTime})
                                        }

                                    })
                                    

                                })
                                

                            })




                            console.log($scope.maindata)



                        }

                      
                    }
                    else {
                        swal('No Record found to display .. !');
                        return;
                    }

                });
            } else {
                //$scope.submitted = false;
            }
        };



        $scope.isOptionsRequired231 = function () {
            return !$scope.employeedropdown.some(function (options) {
                return options.emple;
            });
        }


        //$scope.printData = function () {
        //    var divToPrint = document.getElementById("Table");
        //        var newWin = window.open();
        //        newWin.document.write(divToPrint.outerHTML);
        //        newWin.print();
        //        newWin.close();
        //        // $state.reload();
        //    }

        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };



        $scope.transtypechange = function () {

            $scope.maindata = [];
           
        }

        $scope.printData = function () {
            var ids = '';
            if ($scope.type=='SCH') {
                ids ='printSectionId1'
            }
            else {
                ids = 'printSectionId12'
            }


            var innerContents = document.getElementById(ids).innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/ExchangableCoursePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 1000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.ExportToExcel = function () {
            var ids = '';
            if ($scope.type == 'SCH') {
                ids = '#printSectionId16'
            }
            else {
                ids = '#printSectionId126'
            }
            var exportHref = Excel.tableToExcel(ids, '');
            $timeout(function () { location.href = exportHref; }, 1000);
        }

    }


})();