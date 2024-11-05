
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGRegNoFormatController', CLGRegNoFormatController)

    CLGRegNoFormatController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'superCache']
    function CLGRegNoFormatController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, superCache) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.submitted = false;
        $scope.sortKey = 'imcC_Id';
        $scope.sortReverse = true;
        $scope.dis = false;
        $scope.regformattype = 'auto';
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 10;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            $scope.All_S = false;
            $scope.dis = false;
            $scope.coursebranchlist = [];
            //$scope.amcobM_Id = '';
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            apiService.getDATA("CLGRegNoFormat/getalldetails").
                then(function (promise) {
                    if (promise.datalist != null) {
                        $scope.datalist = promise.datalist;
                        $scope.acrF_Id = $scope.datalist[0].acrF_Id;
                        $scope.acrF_CollegeCodeFlg = $scope.datalist[0].acrF_CollegeCodeFlg;
                        $scope.acrF_CCOrderFlg = $scope.datalist[0].acrF_CCOrderFlg;
                        $scope.acrF_AYCodeFlg = $scope.datalist[0].acrF_AYCodeFlg;
                        $scope.acrF_AYCodeOrderFlg = $scope.datalist[0].acrF_AYCodeOrderFlg;
                        $scope.acrF_BranchCodeFlg = $scope.datalist[0].acrF_BranchCodeFlg;
                        $scope.acrF_BranchCodeOrderFlg = $scope.datalist[0].acrF_BranchCodeOrderFlg;
                        $scope.acrF_NumericWidth = $scope.datalist[0].acrF_NumericWidth;
                        $scope.acrF_SLNo = $scope.datalist[0].acrF_SLNo;
                        $scope.acrF_StartingNo = $scope.datalist[0].acrF_StartingNo;
                        $scope.acrF_PrefilZeroFlg = $scope.datalist[0].acrF_PrefilZeroFlg;

                    }
                    else {

                    }
                })
            //$scope.reverse = false;
            //$scope.sort = function (keyname) {
            //    $scope.sortKey = keyname;   //set the sortKey to the param passed
            //    $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            //}

        };

        $scope.isOptionsRequired1 = function () {

            return !$scope.semlist.some(function (options) {
                return options.checked;
            });

        }









        $scope.toggleAll_S = function () {
            angular.forEach($scope.semlist, function (subj) {
                subj.checked = $scope.All_S;
            })
        };
        $scope.optiontoggled = function () {

            var ccorder = $scope.acrF_CCOrderFlg;
            var yccorder = $scope.acrF_AYCodeOrderFlg;
            var bccorder = $scope.acrF_BranchCodeOrderFlg;

            if ((ccorder == undefined || ccorder == "") && (yccorder == undefined || yccorder == "") && (bccorder == undefined || bccorder == "")) {

            } else {
                if ($scope.acrF_CCOrderFlg == $scope.acrF_AYCodeOrderFlg || $scope.acrF_CCOrderFlg == $scope.acrF_BranchCodeOrderFlg) {
                    swal("Order Can Not Be Same It Should Be Different");
                    return;
                }

                if ($scope.acrF_AYCodeOrderFlg == $scope.acrF_CCOrderFlg || $scope.acrF_AYCodeOrderFlg == $scope.acrF_BranchCodeOrderFlg) {
                    swal("Order Can Not Be Same It Should Be Different");
                    return;
                }

                if ($scope.acrF_BranchCodeOrderFlg == $scope.acrF_AYCodeOrderFlg || $scope.acrF_BranchCodeOrderFlg == $scope.acrF_CCOrderFlg) {
                    swal("Order Can Not Be Same It Should Be Different");
                    return;
                }
            }

            if ($scope.acrF_CollegeCodeFlg == false) {
                $scope.acrF_CCOrderFlg = "";
            }
            if ($scope.acrF_AYCodeFlg == false) {
                $scope.acrF_AYCodeOrderFlg = "";
            }

            if ($scope.acrF_BranchCodeFlg == false) {
                $scope.acrF_BranchCodeOrderFlg = "";
            }

        };

        $scope.optiontoggled1 = function () {
            if ($scope.acrF_CCOrderFlg != undefined && $scope.acrF_CCOrderFlg != "") {
                if ($scope.acrF_CCOrderFlg == $scope.acrF_AYCodeOrderFlg || $scope.acrF_CCOrderFlg == $scope.acrF_BranchCodeOrderFlg) {
                    swal("Order Can Not Be Same It Should Be Different");
                    return;
                }
            }            
        }


        $scope.optiontoggled2 = function () {
            if ($scope.acrF_AYCodeOrderFlg != undefined && $scope.acrF_AYCodeOrderFlg != "") {
                if ($scope.acrF_AYCodeOrderFlg == $scope.acrF_CCOrderFlg || $scope.acrF_AYCodeOrderFlg == $scope.acrF_BranchCodeOrderFlg) {
                    swal("Order Can Not Be Same It Should Be Different");
                    return;
                }
            }
            
        }

        $scope.optiontoggled3 = function () {
            if ($scope.acrF_BranchCodeOrderFlg != undefined && $scope.acrF_BranchCodeOrderFlg != "") {
                if ($scope.acrF_BranchCodeOrderFlg == $scope.acrF_AYCodeOrderFlg || $scope.acrF_BranchCodeOrderFlg == $scope.acrF_CCOrderFlg) {
                    swal("Order Can Not Be Same It Should Be Different");
                    return;
                }
            }
         
        }






        //$scope.chk_valid = function () {
        //    var cnt = 0;
        //    if($scope.acrF_AYCodeFlg)
        //    {
        //        if( Number($scope.acrF_AYCodeOrderFlg) == Number($scope.acrF_CCOrderFlg)){
        //            cnt += 1;
        //        }
        //    }
        //    if ($scope.acrF_BranchCodeFlg) {
        //        if (Number($scope.acrF_BranchCodeOrderFlg) == Number($scope.acrF_CCOrderFlg)) {
        //            cnt += 1;
        //        }
        //    }
        //    if (cnt > 0) {
        //        $scope.acrF_CCOrderFlg = 0;
        //        swal("Can't Set Same Order For Both!!!");
        //    }
        //}



        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }








        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.savetmpldata = function () {

            if ($scope.myForm.$valid) {

                if ($scope.acrF_CCOrderFlg == $scope.acrF_AYCodeOrderFlg || $scope.acrF_CCOrderFlg == $scope.acrF_BranchCodeOrderFlg) {
                    swal("Order Can Not Be Same It Should Be Different");

                    return;
                }

                if ($scope.acrF_AYCodeOrderFlg == $scope.acrF_CCOrderFlg || $scope.acrF_AYCodeOrderFlg == $scope.acrF_BranchCodeOrderFlg) {
                    swal("Order Can Not Be Same It Should Be Different");

                    return;
                }

                if ($scope.acrF_BranchCodeOrderFlg == $scope.acrF_AYCodeOrderFlg || $scope.acrF_BranchCodeOrderFlg == $scope.acrF_CCOrderFlg) {
                    swal("Order Can Not Be Same It Should Be Different");

                    return;
                }



                if ($scope.acrF_CollegeCodeFlg == false) {
                    $scope.acrF_CCOrderFlg = 0;
                }
                if ($scope.acrF_AYCodeFlg == false) {
                    $scope.acrF_AYCodeOrderFlg = 0;
                }

                if ($scope.acrF_BranchCodeFlg == false) {
                    $scope.acrF_BranchCodeOrderFlg = 0;
                }




                var data = {
                    "ACRF_Id": $scope.acrF_Id,
                    "ACRF_CollegeCodeFlg": $scope.acrF_CollegeCodeFlg,
                    "ACRF_CCOrderFlg": $scope.acrF_CCOrderFlg,
                    "ACRF_AYCodeFlg": $scope.acrF_AYCodeFlg,
                    "ACRF_AYCodeOrderFlg": $scope.acrF_AYCodeOrderFlg,
                    "ACRF_BranchCodeFlg": $scope.acrF_BranchCodeFlg,
                    "ACRF_BranchCodeOrderFlg": $scope.acrF_BranchCodeOrderFlg,
                    "ACRF_NumericWidth": $scope.acrF_NumericWidth,
                    "ACRF_SLNo": $scope.acrF_SLNo,
                    "ACRF_StartingNo": $scope.acrF_StartingNo,
                    "ACRF_PrefilZeroFlg": $scope.acrF_PrefilZeroFlg,

                }

                apiService.create("CLGRegNoFormat/Savedetails", data).then(function (promise) {
                    if (promise.returnval == true) {

                        swal("Data Saved/Updated Successfully ")
                        $scope.BindData();


                    }


                    else {
                        swal("Error ")


                    }

                })

                //}
                //else {
                //    swal("order is duplicate")
                //}
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.cancel = function () {
            $scope.BindData();
        }




        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.amB_BranchName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.amcO_CourseName)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
    }

})();