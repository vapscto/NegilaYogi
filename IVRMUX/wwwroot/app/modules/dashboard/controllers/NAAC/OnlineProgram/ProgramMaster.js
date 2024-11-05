

(function () {
    'use strict';
    angular
        .module('app')
        .controller('ProgramMasterController', ProgramMasterController)

    ProgramMasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function ProgramMasterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {

        $scope.groups = [{ title: 'Dynamic Group Header - 1', content: 'Dynamic Group Body - 1' },
        { title: 'Dynamic Group Header - 2', content: 'Dynamic Group Body - 2' },
        { title: 'Dynamic Group Header - 3', content: 'Dynamic Group Body - 3' },
        { title: 'Dynamic Group Header - 4', content: 'Dynamic Group Body - 4' }];

        $scope.searc_button = true;
        $scope.sortKey = 'LMSMOEQ_Id';
        $scope.sortReverse = true;
        $scope.searchValue = "";
        $scope.answer = "";
        $scope.show_ansOption = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        //-------------------Load Data
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("ProgramMaster/getloaddata", pageid).
                then(function (promise) {
                    $scope.currentPage = 1;
                 
                    $scope.Typelist = promise.typelist;
                    $scope.levellist = promise.levellist;
                    $scope.getFQuestiondetails = promise.getFQuestiondetails;
                    $scope.getFQOptiondetails = promise.getFQOptiondetails;
                  
                })
        };


        //------------------1st Tab 
        $scope.savedatalevel = function (levellist) {
            $scope.submitted1 = true;
            $scope.submitted2 = false;
            $scope.submitted3 = false;
            debugger;
            if ($scope.myForm.$valid) {
                var data = {
                    "PRMTLE_Id": $scope.prmtlE_Id,
                    "programname": $scope.Level,
                    "PRMTLE_IdDesc": $scope.Leveldesc,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                debugger;
                apiService.create("ProgramMaster/savedatalevel", data).
                    then(function (promise) {

                        if (promise.message == 'Update') {
                            swal('Record Updated successfully');
                        }
                        else if (promise.message == 'Not Update') {
                            swal('Record Not Updated');
                        }
                        else if (promise.message == 'Duplicate') {
                            swal('Duplicate Record');
                        }
                        else if (promise.message == 'Saved') {
                            swal('Record Saved successfully');
                        }
                        else {
                            swal('Record Not Saved');
                        };
                        $state.reload();
                    })
            }
            else {
                $scope.submitted1 = true;
            }
        };

        //2nd tab save

        $scope.savedatatype = function (Typelist) {
            $scope.submitted2 = true;
            $scope.submitted1 = false;
            $scope.submitted3 = false;
            debugger;
            if ($scope.myForm2.$valid) {
                var data = {
                    "PRMTY_Id": $scope.prmtY_Id,
                    "programname": $scope.type,
                    "PRMTLE_IdDesc": $scope.Typedesc,
                 
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                debugger;
                apiService.create("ProgramMaster/savedatatype", data).
                    then(function (promise) {

                        if (promise.message == 'Update') {
                            swal('Record Updated successfully');
                        }
                        else if (promise.message == 'Not Update') {
                            swal('Record Not Updated');
                        }
                        else if (promise.message == 'Duplicate') {
                            swal('Duplicate Record');
                        }
                        else if (promise.message == 'Saved') {
                            swal('Record Saved successfully');
                        }
                        else {
                            swal('Record Not Saved');
                        }
                        $state.reload();
                    });
            }
            else {
                $scope.submitted2 = true;
            }
        };

        $scope.editlevel = function (id) {
            debugger;
            var data = {
                "PRMTLE_Id": id
            }

            apiService.create("ProgramMaster/editlevel", data).then(function (promise) {
                if (promise.levellist.length > 0) {
                    $scope.prmtlE_Id = promise.levellist[0].prmtlE_Id
                    $scope.Level = promise.levellist[0].prmtlE_ProgramLevel;
                    $scope.Leveldesc = promise.levellist[0].prmtlE_ProgramLevelDes;
                }
            })

        }

        $scope.edittype = function (id) {
            debugger;
            var data = {
                "PRMTY_Id": id
            }

            apiService.create("ProgramMaster/edittype", data).then(function (promise) {
                if (promise.typelist.length > 0) {
                    $scope.prmtY_Id = promise.typelist[0].prmtY_Id;
                    $scope.type = promise.typelist[0].prmtY_ProgramType;
                    $scope.Typedesc = promise.typelist[0].prmtY_ProgramTypeDes;
                  
                }
            })

        }


        $scope.deactivelevel = function (groupHeadData, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
          
            if (groupHeadData.prmtlE_ActiveFlg == 1) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {


                        apiService.create("ProgramMaster/deactivelevel", groupHeadData).
                            then(function (promise) {
                           
                                if (promise.returnval == true) {
                                    swal("Record " + confirmmgs + " Successfully");
                                }
                                else if (promise.message == "used") {
                                    swal("Record already Used");
                                }
                                else {
                                    swal("Record " + confirmmgs + " Successfully");
                                }

                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                })
        }



        $scope.deactivetype = function (groupHeadData, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (groupHeadData.prmtY_ActiveFlg == 1) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {


                        apiService.create("ProgramMaster/deactivetype", groupHeadData).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + confirmmgs + " Successfully");
                                }
                                else if (promise.message == "used") {
                                    swal("Record already Used");
                                }
                                else {
                                    swal("Record " + confirmmgs + " Successfully");
                                }

                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                })
        }

        


        $scope.cancel = function () {
            $state.reload();
        }

        $scope.sort = function (key) {
            $scope.sortKey = key;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }



        $scope.interacted = function (field) {
            return $scope.submitted1;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };
        $scope.interacted3 = function (field) {
            return $scope.submitted3;
        };



        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }

        $scope.search_box = function () {
            if ($scope.searchValue != "" || $scope.searchValue != null) {
                $scope.searc_button = false;
            }
            else {
                $scope.searc_button = true;
            }
        }
        $scope.filterValue2 = function (obj) {
            return (angular.lowercase(obj.asmcL_ClassName)).indexOf(angular.lowercase($scope.searchValue2)) >= 0 ||
                (angular.lowercase(obj.LMSMOEQ_Marks)).indexOf(angular.lowercase($scope.searchValue2)) >= 0;
        }

        

        $scope.cancel1 = function (user) {
            $scope.ques_id = "";
            $scope.loaddata();
            $scope.show_ansOption = false;
        }


        $scope.search_box1 = function () {
            if ($scope.searchValue1 != "" || $scope.searchValue1 != null) {
                $scope.searc_button1 = false;
            }
            else {
                $scope.searc_button1 = true;
            }
        }
        $scope.filterValue1 = function (obj) {
            return (angular.lowercase(obj.LMSMOEQ_Question)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.LMSMOEQ_QuestionDesc)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (JSON.stringify(obj.LMSMOEQ_Marks)).indexOf($scope.searchValue) >= 0;
        }



       

    }
})();