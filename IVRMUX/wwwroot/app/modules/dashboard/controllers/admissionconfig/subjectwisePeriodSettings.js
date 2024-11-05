
(function () {
    'use strict';
    angular
.module('app')
.controller('subjectwisePeriodSettingsController', subjectwisePeriodSettingsController)

    subjectwisePeriodSettingsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function subjectwisePeriodSettingsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


      
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        //$scope.pagination = false;
       // $scope.IsHiddendown = false;
        $scope.propertyName = 'subjectName';
        $scope.gridviewList = [];

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };                                            
        $scope.ShowHidedown = function () {


            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        }

        $scope.pageChanged = function (newPage) {
            if (newPage > 0) {
                $scope.newPage = newPage;


            }
        };                                
       

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.BindData = function () {
            apiService.getDATA("SubjectwisePeriodSettings/Getdetails").
       then(function (promise) {
           $scope.arrlist2 = promise.yeardropDown;
           $scope.currYear = promise.currentAcdYear;
           for (var i = 0; i < $scope.arrlist2.length; i++) {
               name = $scope.arrlist2[i].asmaY_Id;
               for (var j = 0; j < $scope.currYear.length; j++) {
                   if (name == $scope.currYear[j].asmaY_Id) {
                       $scope.arrlist2[i].Selected = true;
                       $scope.ASMAY_Id = $scope.currYear[j].asmaY_Id;
                   }
               }
           }
           $scope.arrlist4 = promise.classdropDown;
           $scope.arrlist5 = promise.sectiondropDown;
           if (promise.count > 0)
           {
               $scope.gridviewList = promise.gridviewList;
               angular.forEach($scope.gridviewList, function (value1, i) {
                   $scope.gridviewList[i].asasmP_MaxPeriod = 0;
               });
               $scope.IsHiddendown = true;
           }
           else {
               swal("No Subjects Are Created.Please Create Subject & Then Proceed.");
               $scope.IsHiddendown = false;
           }
         

           //$scope.pagination = true;
       })
        };

        $scope.isOptionsRequired = function () {
            return !$scope.arrlist5.some(function (options) {
                return options.Selected;
            });
        }

        $scope.checkboxchcked = [];

        $scope.CheckedSectionName = function (data) {
            
            $scope.ASMS_Id =data.asmS_Id;
            $("input:checkbox").on('click', function () {
                // in the handler, 'this' refers to the box clicked on
                var $box = $(this);
                if ($box.is(":checked")) {
                    // the name of the box is retrieved using the .attr() method
                    // as it is assumed and expected to be immutable
                    var group = "input:checkbox[name='" + $box.attr("name") + "']";
                    // the checked state of the group/box on the other hand will change
                    // and the current value is retrieved using .prop() method
                    $(group).prop("checked", false);
                    $box.prop("checked", true);
                } else {
                    $box.prop("checked", false);
                }
            });
            
            if ($scope.checkboxchcked.indexOf(data) === -1) {
                
                if ($scope.ASMAY_Id > 0 && $scope.ASMCL_Id > 0 && $scope.ASMS_Id > 0)
                {
                    $scope.Subject_MaxPeriod();
                }

                //  $scope.checkboxchcked.push(data);
                

            }
            else {
                $scope.checkboxchcked.splice($scope.checkboxchcked.indexOf(data), 1);
            }
        }


        $scope.isOptionsRequired = function () {
            return !$scope.arrlist5.some(function (options) {
                return options.Selected;
            });
        }




        $scope.onyearchange=function()
        {
            if ($scope.ASMAY_Id > 0 && $scope.ASMCL_Id > 0 && $scope.ASMS_Id > 0) {
                $scope.Subject_MaxPeriod();
            }
        }
        $scope.onclassChange=function()
        {
            if ($scope.ASMAY_Id > 0 && $scope.ASMCL_Id > 0 && $scope.ASMS_Id > 0) {
                $scope.Subject_MaxPeriod();
            }
        }

        $scope.Subject_MaxPeriod = function () {

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMC_Id": $scope.ASMS_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("SubjectwisePeriodSettings/subjectMaxPeriod/", data).
           then(function (promise) {
               
               if (promise.subjectwisePeriodCount.length > 0)
               {
                   angular.forEach(promise.subjectwisePeriodCount, function (value1, i) {
                       $scope.gridviewList[i].asasmP_MaxPeriod = promise.subjectwisePeriodCount[i].asasmP_MaxPeriod;
                   });
               }
               else {
                   angular.forEach($scope.gridviewList, function (value1, i) {
                       $scope.gridviewList[i].asasmP_MaxPeriod = 0;
                   });
               }
               

           })
        }

        $scope.submitted = false;
        $scope.savedata = function (user) {
            
            if ($scope.myForm.$valid) {
                var SectionsIDs = $scope.checkboxchcked;
                //if ($scope.ASMS_Id == "" || $scope.ASMS_Id == undefined || $scope.ASMS_Id==null) {
                //    swal("Select Section Checkbox");
                //    return;
                //}
                angular.forEach(user, function (value1, i) {
                    if (user[i].asasmP_MaxPeriod == "")
                    {
                        user[i].asasmP_MaxPeriod = 0;
                    }
                });
                
                if ($scope.myForm.$valid) {
                    
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMC_Id": $scope.ASMS_Id,
                        "SelectedSubjectMaxPeriods": user,
                    };
                    apiService.create("SubjectwisePeriodSettings/", data).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal("Record Saved/Updated Successfully");
                        $state.reload();
                    }
                    else {
                        swal("Failed to Save/Update the Record");
                    }
                });
                }
            }else
            {
                $scope.submitted = true;
            }
        }
        $scope.clear=function()
        {
            $state.reload();
        }
        $scope.interacted = function (field) {

            return $scope.submitted;
        };
    
    }

})();