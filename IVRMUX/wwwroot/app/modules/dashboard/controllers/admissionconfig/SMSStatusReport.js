
(function () {
    'use strict';
    angular
.module('app')
        .controller('SMSStatusReportController', SMSStatusReportController)

    SMSStatusReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache', '$filter', 'Excel','$timeout']
    function SMSStatusReportController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache, $filter, Excel, $timeout) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.sortKey = 'ssdnO_Id';
        $scope.sortReverse = true;
        $scope.currentPage = 1;
        $scope.search = '';
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.itemsPerPage = 10;
        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));


        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.headerlist, function (itm) {
                itm.select = checkStatus;
            });

            $scope.onheadchenge();
            if ($scope.usercheck == false) {
                $scope.usercheck23 = "";
                angular.forEach($scope.transnolist, function (obj) {
                    obj.select = false;
                });
            }
            else if ($scope.usercheck == true) {
                angular.forEach($scope.transnolist, function (obj) {
                    obj.select = true;
                });
                $scope.togchkbx23();
            }


        }

          $scope.filterchkbx = function (obj) {
              return (angular.lowercase(obj.ssD_HeaderName)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.headerlist.every(function (options) {
                return options.select;
            });

            $scope.onheadchenge();
        }

        $scope.isOptionsRequired = function () {
            return !$scope.headerlist.some(function (options) {
                return options.select;
            });
        }



        $scope.searchchkbx23 = "";
        $scope.all_check23 = function () {
            var checkStatus = $scope.usercheck23;
            angular.forEach($scope.transnolist, function (itm) {
                itm.select = checkStatus;
            });
        }

        $scope.togchkbx23 = function () {
            $scope.usercheck23 = $scope.transnolist.every(function (options) {
                return options.select;
            });
        }

        $scope.isOptionsRequired23 = function () {
            return !$scope.transnolist.some(function (options) {
                return options.select;
            });
        }

        $scope.filterchkbx23 = function (obj) {
            return (angular.lowercase(obj.transname)).indexOf(angular.lowercase($scope.searchchkbx23)) >= 0;
        }



        $scope.searchchkbx231 = "";
        $scope.all_check231 = function () {
            var checkStatus = $scope.usercheck231;
            angular.forEach($scope.statuslist, function (itm) {
                itm.select = checkStatus;
            });
        }

        $scope.togchkbx231 = function () {
            $scope.usercheck231 = $scope.statuslist.every(function (options) {
                return options.select;
            });
        }

        $scope.isOptionsRequired231 = function () {
            return !$scope.statuslist.some(function (options) {
                return options.select;
            });
        }

        $scope.filterchkbx231 = function (obj) {
            return (angular.lowercase(obj.ssdnO_Status)).indexOf(angular.lowercase($scope.searchchkbx231)) >= 0;
        }











        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("SMSResend/Getdetailsstatus").
       then(function (promise) {
           $scope.headerlist = promise.headerlist;
           $scope.statuslist = promise.statuslist;
       })
        };

        $scope.transchange = function () {
            $scope.msgstatus = '';
            $scope.showg = false;
        }
        var heads = [];
        $scope.onheadchenge = function () {
            debugger;
            angular.forEach($scope.headerlist, function (rr) {
                debugger;
                if (rr.select == true) {
                    heads.push({ SSD_HeaderName: rr.ssD_HeaderName});
                }

            })

            $scope.msgstatus = '';
            $scope.ssD_TransactionId = '';
            $scope.showg = false;
           
                var data = {
                    headsname: heads,
                }
                apiService.create("SMSResend/Gettransnostatus", data).
                    then(function (promise) {
                        debugger;
                        $scope.transnolist = promise.transnolist;
                        
                    })
            //}

        }

        //delete record
        $scope.Deletemastercastedata = function (DeleteRecord, SweetAlert) {
            
            $scope.deleteId = DeleteRecord.iC_Id;
            var MdeleteId = $scope.deleteId;

            // swal(id);
            swal({
                title: "Are you sure?",
                text: "Do you want to delete the Caste ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("mastercaste/MasterDeleteModulesDTO", MdeleteId).
                    then(function (promise) {
                        if (promise.msg != "" && promise.msg != null) {
                            swal(promise.msg);
                            return;
                        }
                        if (promise.returnVal == true) {
                            swal("Caste Deleted Successfully");
                            $state.reload();
                        }
                        else {
                            swal("Failed to Delete the Caste");
                        }
                    })
                }
                else {
                    swal("Caste Deletion Cancelled");
                }

            });
        }

        // to Edit Data
      

        $scope.interacted = function (field) {

            return $scope.submitted;
        };


        ///check all
        $scope.printstudents = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.fillgriddata, function (itm) {
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

            $scope.all = $scope.fillgriddata.every(function (options) {

                return options.selected;
            });

            //$scope.all = $scope.employeelst.every(function (itm)
            //{ return itm.selected; });
            //if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
            //    $scope.printstudents.push(SelectedStudentRecord);
            //}
            //else {
            //    $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            //}
        }

        $scope.sort = function (key) {
            $scope.reverse = ($scope.sortKey == key) ? !$scope.reverse : $scope.reverse;
            $scope.sortKey = key;
        }

        //To Resend 
        $scope.ResendSMS = function () {
            $scope.albumNameArray = [];
            //$scope.showg = false;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                debugger;
              
                angular.forEach($scope.fillgriddata, function (user) {
                    if (!!user.selected) {
                        $scope.albumNameArray.push(user);
                    }
                })

                if ($scope.albumNameArray.length > 0) {
                    var data = {
                        "SSD_HeaderName": $scope.ssD_HeaderName,
                        "SSD_TransactionId": $scope.ssD_TransactionId,
                        "resenddata": $scope.albumNameArray
                    }
                    apiService.create("SMSResend/resendMsg", data).
                        then(function (promise) {
                            if (promise.retmsg =='error') {
                                swal('Error in Resend OR Status Update');
                            }
                            else {
                                swal('Successfully SMS Resend OR Status Updated');
                                $scope.showdata(); 
                            }
                          

                        })
                }
                else {
                    swal('Select Atleast one Record')
                }
               
            }
        };

        $scope.checkst = '';

        $scope.selectstatus = function () {
            $scope.checkst = 'status';
            if ($scope.myForm.$valid) {

                debugger;
                var data = {
                    "FromDate": $scope.FromDate,
                    "ToDate": $scope.ToDate,
                    "SSD_HeaderName": $scope.ssD_HeaderName,
                    "SSD_TransactionId": $scope.ssD_TransactionId,
                    "msgstatus": $scope.msgstatus,
                    "checkst": $scope.checkst,

                }
                apiService.create("SMSResend/showdata", data).
                    then(function (promise) {
                        $scope.fillgriddata = promise.fillgriddata;
                        $scope.fillsentdata = promise.fillsentdata;
                        $scope.messgetext = $scope.fillsentdata[0].ssdN_SMSMessage;
                        $scope.messcount = $scope.fillsentdata.length;
                        $scope.messsntdate = $scope.fillsentdata[0].ssD_SentDate;
                        $scope.messsnttime = $scope.fillsentdata[0].ssD_Senttime;
                        if ($scope.fillgriddata.length > 0) {
                            $scope.showg = true;
                        }
                        else {
                            swal('No Record Found');
                        }


                    })
            }

        }


        // TO show The Data
        $scope.fillgriddata = [];
        $scope.showg = false;
        $scope.submitted = false;
        $scope.showdata = function () {
            $scope.checkst = 'show';
            $scope.fillgriddata = [];
            $scope.fillsentdata = [];
            $scope.showg = false;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var heads1 = [];
                var trans1 = [];
                var status1 = [];
                angular.forEach($scope.headerlist, function (rr) {
                    debugger;
                    if (rr.select == true) {
                        heads1.push({ SSD_HeaderName: rr.ssD_HeaderName });
                    }

                })
                angular.forEach($scope.transnolist, function (rr) {
                    debugger;
                    if (rr.select == true) {
                        trans1.push({ SSD_TransactionId: rr.ssD_TransactionId });
                    }

                })
                angular.forEach($scope.statuslist, function (rr) {
                    debugger;
                    if (rr.select == true) {
                        status1.push({ SSDNO_Status: rr.ssdnO_Status });
                    }

                })

                var fromdate1 = $scope.FromDate == null ? "" : $filter('date')($scope.FromDate, "yyyy-MM-dd");
                var todate1 = $scope.ToDate == null ? "" : $filter('date')($scope.ToDate, "yyyy-MM-dd");


                var data = {
                    "FromDate": fromdate1,
                    "ToDate": todate1,
                    headsname: heads1,
                    transnameid: trans1,
                    statusname: status1,
                }
                apiService.create("SMSResend/getstatusreport", data).
                    then(function (promise) {
                        $scope.fillgriddata = promise.fillgriddata;


                        if ($scope.fillgriddata.length > 0) {

                            //angular.forEach($scope.fillgriddata, function (ee) {

                            //    ee.SSD_Senttime = moment(ee.SSD_Senttime, 'h:mm a').format();

                            //})

                         

                            $scope.showg = true;
                        }
                        else {
                            swal('No Record Found');
                        }
                        
                      
                    })
            }
        };
        //===========print===========//
        $scope.printData = function () {
            var innerContents = document.getElementById("printtable").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/BookTypeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }













        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }

        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        }

        $scope.cancel = function () {
            $state.reload();
        }

        $scope.searchValue = "";
     

    }

})();