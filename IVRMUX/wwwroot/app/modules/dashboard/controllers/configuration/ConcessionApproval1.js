

(function () {
    'use strict';
    angular
.module('app')
        .controller('ConcessionApprovalController', ConcessionApprovalController)

    ConcessionApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','superCache']
    function ConcessionApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.checkboxchcked = [];

        $scope.loadData = function () {
            var pageid = 2;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            apiService.getURI("ConcessionApproval/loaddata", pageid).
            then(function (promise) {
                $scope.catdrp = promise.fillcategory
                $scope.studentdrp = promise.fillstudentlst
            })

            $scope.order = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }

        $scope.cleardata=function(){
          
            $scope.FMCC_Id = "";
            $scope.pasr_id = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }

        $scope.onstudchange = function () {
            $scope.secondgrid = false;
            $scope.firstgrid = false;

            $scope.searcgbtn = false;
        }

        $scope.oncatchange = function () {

            var listOfStu = {
                "FMCC_Id": $scope.FMCC_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            //var catid = $scope.FMCC_Id;
            apiService.create("ConcessionApproval/catchange", listOfStu).
            then(function (promise) {
                $scope.studentdrp = promise.fillstudentlst
                $scope.checkbutton =false;
                $scope.searcgbtn = false;
            })
        }

        $scope.submitted = false;
        $scope.searchdata = function () {
            if ($scope.myForm.$valid) {
                $scope.firstgrid = false;

                var listOfStu = {
                    "FMCC_Id": $scope.FMCC_Id,
                    "PASR_Id": $scope.pasr_id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("ConcessionApproval/oncheck", listOfStu).
                then(function (promise) {
                    if (promise.fillstudentlst.length > 0) {
                        $scope.firstgrid = true;
                        $scope.checkbutton = false;
                        $scope.secondgrid = false;

                        $scope.displaysiblingdet = promise.fillstudentlst;
                        swal('Record Checked Successfully', '');
                    }
                    else {
                        swal('No Records');
                    }

                })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.savedatatrans = [];
        $scope.checkdata = function (displaysiblingdet) {
            $scope.savedatatrans = [];
            var count;
            angular.forEach($scope.displaysiblingdet, function (user) {
                if (!!user.isSelected) {
                    $scope.savedatatrans.push(user);
                    count = 1;
                }
            })

            if (count == 1)
            {
                var listOfStu = {
                    "FMCC_Id": $scope.FMCC_Id,
                    "PASR_Id": $scope.pasr_id,
                    "confirmorrejectstatus": 'Check',
                    studentdetails: $scope.savedatatrans
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("ConcessionApproval/saveconfirmdata", listOfStu).
                   then(function (promise) {
                       $scope.labelstatus = true;
                       $scope.secondgrid = true;
                     
                       $scope.checkbutton = true;
                       $scope.searcgbtn = true;

                       $scope.displayselectedlst = promise.studentdetails;
                   })
            }

            else
            {
                swal('Kindly select any one student');
            }
           
        }

        $scope.savedatatrans1 = [];
        $scope.confirmdata = function (displayselectedlst) {

            var count;
            angular.forEach($scope.displayselectedlst, function (user) {
                if (!!user.isSelected) {
                    $scope.savedatatrans1.push(user);
                    count = 1;
                }
            })

            if (count == 1)
            {
                var listOfStu = {
                    "FMCC_Id": $scope.FMCC_Id,
                    "PASR_Id": $scope.pasr_id,
                    "confirmorrejectstatus": 'C',
                    studentdetails: $scope.savedatatrans1
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("ConcessionApproval/saveconfirmdata", listOfStu).
                   then(function (promise) {
                       if (promise.returnval == true) {
                           swal("Record Confirmed Successfully");
                       }
                       else if (promise.returnval == false) {
                           swal("Record Not Saved Successfully");
                       }

                       $state.reload();

                   })
            }

            else {
                swal('Kindly select any one student');
            }

          
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.savedatatrans2 = [];
        $scope.rejectdata = function (displaysiblingdet) {

            angular.forEach($scope.displaysiblingdet, function (user) {
                if (!!user.isSelected) {
                    $scope.savedatatrans2.push(user);
                }
            })

            var listOfStu = {
                "FMCC_Id": $scope.FMCC_Id,
                "PASR_Id": $scope.pasr_id,
                "confirmorrejectstatus": 'R',
                studentdetails: $scope.savedatatrans2
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("ConcessionApproval/saveconfirmdata", listOfStu).
               then(function (promise) {
                   if (promise.returnval == true) {
                       swal("Record Rejected Successfully");
                   }
                   else if (promise.returnval == false) {
                       swal("Record Not Saved Successfully");
                   }

                   $state.reload();
               })
        }

    }
})();
