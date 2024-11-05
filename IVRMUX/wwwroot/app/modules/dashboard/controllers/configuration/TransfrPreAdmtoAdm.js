//dashboard.controller("TransfrPreAdmtoAdmController", ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash',
//function ($rootScope, $scope, $state, $location, dashboardService, Flash) {
//    $scope.predicate = 'sno';
//    $scope.reverse = false;
//    $scope.currentPage = 1;
//    $scope.order = function (predicate) {
//        $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
//        $scope.predicate = predicate;
//    };
//    $scope.students = [
//      { sno: '1', name: 'Kevin', age: 25, gender: 'boy' },
//      { sno: '2', name: 'John', age: 30, gender: 'girl' },
//      { sno: '3', name: 'Laura', age: 28, gender: 'girl' },
//      { sno: '4', name: 'Joy', age: 15, gender: 'girl' },
//      { sno: '5', name: 'Mary', age: 28, gender: 'girl' },
//      { sno: '6', name: 'Peter', age: 95, gender: 'boy' },
//      { sno: '7', name: 'Bob', age: 50, gender: 'boy' },
//      { sno: '8', name: 'Erika', age: 27, gender: 'girl' },
//      { sno: '9', name: 'Patrick', age: 40, gender: 'boy' },
//      { sno: '10', name: 'Tery', age: 60, gender: 'girl' }
//    ];
//    $scope.totalItems = $scope.students.length;
//    $scope.numPerPage = 5;
//    $scope.paginate = function (value) {
//        var begin, end, index;
//        begin = ($scope.currentPage - 1) * $scope.numPerPage;
//        end = begin + $scope.numPerPage;
//        index = $scope.students.indexOf(value);
//        return (begin <= index && index < end);
//    };
(function () {
    'use strict';
    angular
.module('app')
        .controller('TransfrPreAdmtoAdmController', TransfrPreAdmtoAdmController)

    TransfrPreAdmtoAdmController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache']
    function TransfrPreAdmtoAdmController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache) {
        //Date:23-12-2016 for displaying privileges.

        var configsettings = JSON.parse(localStorage.getItem("configsettings"));

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings!=null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.sortKey = "pasR_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;


        $scope.checkboxchcked = [];
        $scope.isOptionsRequired = function () {
            return !$scope.preAdmtoAdmStuList.some(function (options) {
                return options.isSelected;
            });
        }

        //$scope.toggleAll = function () {
        //    var toggleStatus = $scope.all2;
        //    angular.forEach($scope.preAdmtoAdmStuList, function (itm) { itm.isSelected = toggleStatus; });

        //}


        $scope.checkboxselected = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all2;
            angular.forEach($scope.preAdmtoAdmStuList, function (itm) {
                itm.isSelected = toggleStatus;
                if ($scope.all2 == true) {
                    if ($scope.checkboxselected.indexOf(itm) === -1) {
                        $scope.checkboxselected.push(itm);
                    }
                }
                else {
                    $scope.checkboxselected.splice(itm);
                }
            });

        }

       
        $scope.optionToggled = function (data) {
            
            $scope.all2 = $scope.preAdmtoAdmStuList.every(function (itm) { return itm.isSelected; })
            if ($scope.checkboxselected.indexOf(data) === -1) {
                var arr1 = {
                    "pasR_Id": data.pasR_Id
                }
                $scope.checkboxselected.push(arr1);
            }
            else {
                $scope.checkboxselected.splice($scope.checkboxselected.indexOf(data), 1);
            }
        }
        $scope.loadData = function () {
            var pageid = 2;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            apiService.getURI("TransfrPreAdmtoAdm/TrnfPreadmtoAdm", pageid).
            then(function (promise) {

                if ($scope.ASMAY_Id === promise.fillacademicyr[0].asmaY_Id) {
                }
                else {
                    $scope.academicdrp = promise.fillacademicyr;
                    $scope.ASMAY_Id = promise.fillacademicyr[0].asmaY_Id;
                }

                $scope.classdrpDwn = promise.fillclass;
            })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }

        $scope.cleardata = function () {
            $scope.firstgrid = false;
            $scope.obj = {};
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        $scope.submitted = false;
        $scope.submitted1 = false;
        $scope.searchdata = function (obj) {
            $scope.all2 = false;
            if ($scope.myForm.$valid) {
                $scope.firstgrid = false;

                var listOfStu = {
                    "ASMAY_Id": $scope.obj.ASMAY_Id,
                    "ASMCL_Id": $scope.obj.ASMCL_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("TransfrPreAdmtoAdm/searchdata", listOfStu).
              then(function (promise) {
                  $scope.firstgrid = true;
               


                  $scope.albumNameArray1 = [];
                  for (var i = 0; i < promise.preAdmtoAdmStuList.length; i++) {
                      if (promise.preAdmtoAdmStuList[i].amsT_FirstName != '') {
                          if (promise.preAdmtoAdmStuList[i].amsT_MiddleName != null && promise.preAdmtoAdmStuList[i].amsT_MiddleName != '' && promise.preAdmtoAdmStuList[i].amsT_MiddleName != "") {
                              if (promise.preAdmtoAdmStuList[i].amsT_LastName != null && promise.preAdmtoAdmStuList[i].amsT_LastName != '' && promise.preAdmtoAdmStuList[i].amsT_LastName != "") {

                                  $scope.albumNameArray1.push({ name: promise.preAdmtoAdmStuList[i].amsT_FirstName + " " + promise.preAdmtoAdmStuList[i].amsT_MiddleName + " " + promise.preAdmtoAdmStuList[i].amsT_LastName, pasR_Id: promise.preAdmtoAdmStuList[i].pasR_Id });
                              }
                              else {
                                  $scope.albumNameArray1.push({ name: promise.preAdmtoAdmStuList[i].amsT_FirstName + " " + promise.preAdmtoAdmStuList[i].amsT_MiddleName, pasR_Id: promise.preAdmtoAdmStuList[i].pasR_Id });
                              }
                          }
                          else {
                              if (promise.preAdmtoAdmStuList[i].amsT_LastName != null && promise.preAdmtoAdmStuList[i].amsT_LastName != '' && promise.preAdmtoAdmStuList[i].amsT_LastName != "") {
                                  $scope.albumNameArray1.push({ name: promise.preAdmtoAdmStuList[i].amsT_FirstName + " " + promise.preAdmtoAdmStuList[i].amsT_LastName, pasR_Id: promise.preAdmtoAdmStuList[i].pasR_Id });
                              }
                              else {
                                  $scope.albumNameArray1.push({ name: promise.preAdmtoAdmStuList[i].amsT_FirstName, pasR_Id: promise.preAdmtoAdmStuList[i].pasR_Id });
                              }
                          }

                          promise.preAdmtoAdmStuList[i].name = $scope.albumNameArray1[i].name;
                      }
                  }


                  $scope.preAdmtoAdmStuList = promise.preAdmtoAdmStuList;



                  $scope.presentCountgrid = promise.preAdmtoAdmStuList.length;
                  if (promise.preAdmtoAdmStuList.length > 0) {
                     // swal('Record searched Successfully', '');
                  }
                  else {
                      swal('No Records', '');
                      $scope.firstgrid = false;
                  }

              })
            }
            else {
                $scope.submitted = true;
            }


        }

        for (var i = 0; i < configsettings.length; i++) {
            if (configsettings.length > 0) {
                
                $scope.configurationsettings = configsettings[i];

            }
        }


        $scope.savedatatrans = [];
        $scope.exporttoadmissiondata = function (preAdmtoAdmStuList) {

            $scope.savedatatrans = [];
            angular.forEach($scope.preAdmtoAdmStuList, function (user) {
                if (user.isSelected) {
                    $scope.savedatatrans.push(user);
                }
            })
            if ($scope.checkboxselected.length > 0) {
                var listOfStu = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    studentdetails: $scope.savedatatrans,
                    configurationsettings: $scope.configurationsettings
                };

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };

                swal({
                    title: "Are you sure?",
                    text: "Do you want to transfer to admission ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel..!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("TransfrPreAdmtoAdm/exporttoadmission", listOfStu).
                                then(function (promise) {
                                    if (promise.returnMsg != null && promise.returnMsg != "") {
                                        if (promise.returnMsg == "true") {
                                            swal('Record Saved/Updated Successfully', '');
                                            $state.reload();
                                        }
                                        else if (promise.returnMsg == "false") {
                                            swal('Record Not Saved/Updated', '');
                                        }
                                        else {
                                            swal(promise.returnMsg);
                                        }

                                    }
                                });
                        }
                        else {
                            swal("Record Transfer Cancelled");
                        }

                    });
            } else {

                swal('Kindly select atleast one student..!');
                return;
            }
        };
    }
})();
