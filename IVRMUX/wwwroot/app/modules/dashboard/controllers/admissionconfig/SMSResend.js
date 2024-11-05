
(function () {
    'use strict';
    angular
.module('app')
        .controller('SMSResendController', SMSResendController)

    SMSResendController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache','$filter']
    function SMSResendController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache, $filter) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.maxDatemf = new Date();
        $scope.sortKey = 'ssdnO_Id';
        $scope.sortReverse = true;
        $scope.currentPage = 1;
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

        $scope.gettodate = function () {

            $scope.minDatemf = new Date($scope.FromDate);
            $scope.maxDatemf = new Date();
        };

        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("SMSResend/Getdetails").
       then(function (promise) {
           $scope.headerlist = promise.headerlist;
       })
        };

        $scope.transchange = function () {
            $scope.msgstatus = '';
            $scope.showg = false;
        }
        $scope.onheadchenge = function () {
            debugger;
          //  $scope.submitted = true;
            //if ($scope.myForm.$valid) {

            $scope.msgstatus = '';
            $scope.ssD_TransactionId = '';
            $scope.showg = false;
            
                var data = {
                    "SSD_HeaderName": $scope.ssD_HeaderName,
                }
                apiService.create("SMSResend/Gettransno", data).
                    then(function (promise) {

                        $scope.transnolist = promise.transnolist;
                        if ($scope.transnolist.length == 0) {
                            swal('No Record found')
                        }
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
                var fromdate1 = $filter('date')($scope.FromDate, 'dd-MM-yyyy');
                var todate1 = $filter('date')($scope.ToDate, 'dd-MM-yyyy');
                debugger;
                var data = {
                    "FromDate": fromdate1,
                    "ToDate": todate1,
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
        $scope.showg = false;
        $scope.submitted = false;
        $scope.showdata = function () {
            $scope.checkst = 'show';
            $scope.fillgriddata = [];
            $scope.fillsentdata = [];
            $scope.showg = false;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {


                var fromdate1 = $filter('date')($scope.FromDate, 'dd-MM-yyyy');
                var todate1 = $filter('date')($scope.ToDate, 'dd-MM-yyyy');
                debugger;
                var data = {
                    "FromDate": fromdate1,
                    "ToDate": todate1,
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
        };

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