(function () {
    'use strict';
    angular
        .module('app')
        .controller('NAAC_MC_443_BandWidth_Range', NAAC_MC_443_BandWidth_Range)

    NAAC_MC_443_BandWidth_Range.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce', 'myFactorynaac'];
    function NAAC_MC_443_BandWidth_Range($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce, myFactorynaac) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.submitted = false;
        var miid = myFactorynaac.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        $scope.NCMC443BWR_Id = 0;
        $scope.instit = false;
        //======================page load
        $scope.loaddata = function () {
           
            $scope.NCMC443BWR_Id = 0;
            $scope.change_institution();
            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }
            //var pageid = 2;
            apiService.getURI("NAAC_MC_443_BandWidth_Range/loaddata", $scope.mI_Id).then(function (promise) {

               
                $scope.institutionlist = promise.institutionlist;
                $scope.mI_Id = promise.mI_Id;
                $scope.allacademicyear = promise.allacademicyear;

                $scope.alldata1 = promise.alldata1;

                angular.forEach($scope.alldata1, function (aa) {

                    //if (aa.ncmC443BWR_OneOrMoreGBPS == true) {
                    //    $scope.aaaa = "Yes";
                    //    $scope.bbbb = "No";
                    //    $scope.cccc = "No";
                    //    $scope.dddd = "No";
                    //    $scope.eeee = "No";

                    //    $scope.NCMC443BWR_Range = aa.ncmC443BWR_Range;
                    //}
                    //else if (aa.ncmC443BWR_500MBPSTo1GBPS == true) {
                    //    $scope.aaaa = "No";
                    //    $scope.bbbb = "Yes";
                    //    $scope.cccc = "No";
                    //    $scope.dddd = "No";
                    //    $scope.eeee = "No";
                    //    $scope.NCMC443BWR_Range = aa.ncmC443BWR_Range;
                    //}
                    //else if (aa.ncmC443BWR_250MBPSTo500MBPS == true) {
                    //    $scope.aaaa = "No";
                    //    $scope.bbbb = "No";
                    //    $scope.cccc = "Yes";
                    //    $scope.dddd = "No";
                    //    $scope.eeee = "No";
                    //    $scope.NCMC443BWR_Range = aa.ncmC443BWR_Range;
                    //}
                    //if (aa.ncmC443BWR_50MBPSTo250MBPS == true) {
                    //    $scope.aaaa = "No";
                    //    $scope.bbbb = "No";
                    //    $scope.cccc = "No";
                    //    $scope.dddd = "Yes";
                    //    $scope.eeee = "No";
                    //    $scope.NCMC443BWR_Range = aa.ncmC443BWR_Range;
                    //}
                    //if (aa.ncmC443BWR_LessThan50MBPS == true) {
                    //    $scope.aaaa = "No";
                    //    $scope.bbbb = "No";
                    //    $scope.cccc = "No";
                    //    $scope.dddd = "No";
                    //    $scope.eeee = "Yes";
                    //    $scope.NCMC443BWR_Range = aa.ncmC443BWR_Range;
                   // }
                })
            })
        };

        //
        $scope.show1 = function () {
     
            $scope.NCMC443BWR_Range = ">=1GBPS";
        }
        $scope.show2 = function () {
          
            $scope.NCMC443BWR_Range = "500MBPS-1GBPS";
        }
        $scope.show3 = function () {
           
            $scope.NCMC443BWR_Range = "250MBPS-500MBPS";
        }
        $scope.show4 = function () {
          
            $scope.NCMC443BWR_Range = "50MBPS-250MBPS";
        }
        $scope.show5 = function () {
           
            $scope.NCMC443BWR_Range = "<50MBPS";
        }

        //======================Record Save
        $scope.search = "";
        $scope.save = function () {
            if ($scope.myForm.$valid) {
              
                if ($scope.flag == 'a') {
                    $scope.flag = "a";
                }
                else if ($scope.flag == 'b') {
                    $scope.flag = "b";
                }
                else if ($scope.flag == 'c') {
                    $scope.flag = "c";
                }
                else if ($scope.flag == 'd') {
                    $scope.flag = "d";
                }
                else if ($scope.flag == 'e') {
                    $scope.flag = "e";
                }

              
                var data = {
                    "NCMC443BWR_Id": $scope.NCMC443BWR_Id,
                    "asmaY_Id": $scope.NCMC443BWR_Year,
                    "NCMC443BWR_Range": $scope.NCMC443BWR_Range,
                    "flag": $scope.flag,
                    
                  
                    "MI_Id": $scope.mI_Id
                }

                apiService.create("NAAC_MC_443_BandWidth_Range/save", data).then(function (promise) {
                  
                    if (promise.msg == 'saved') {
                        swal("Data Saved Successfully...!!!")
                        $state.reload();
                    }
                    else if (promise.msg == 'Failed') {
                        swal("Data Not Saved Successfully...!!!");
                    }
                    else if (promise.msg == 'updated') {
                        swal("Data Updated Successfully...!!!");
                        $state.reload();
                    }
                    else if (promise.msg == 'failed') {
                        swal("Data Not Updated Successfully...!!!");
                    }
                    else if (promise.returnval == true) {
                        swal("Data Already Exists");
                    }
                    else {
                        swal("Something is Wrong......");
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }

   
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   //set the propertyName to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //================================edit data
        $scope.edittab1 = function (user) {
        
            var data = {
                "NCMC443BWR_Id": user.ncmC443BWR_Id,
                "MI_Id": user.mI_Id
            }
            apiService.create("NAAC_MC_443_BandWidth_Range/EditData", data).then(function (promise) {
          
                if (promise.editlist.length > 0) {
                    $scope.instit = true;
                    $scope.NCMC443BWR_Id = promise.editlist[0].ncmC443BWR_Id;
                    $scope.NCMC443BWR_Year = promise.editlist[0].ncmC443BWR_Year;
                    $scope.NCMC443BWR_Range = promise.editlist[0].ncmC443BWR_Range;

                    if (promise.editlist[0].ncmC443BWR_OneOrMoreGBPS == true) {
                        $scope.flag = "a";
                    }
                    if (promise.editlist[0].ncmC443BWR_500MBPSTo1GBPS == true) {
                        $scope.flag = "b";
                    }
                    if (promise.editlist[0].ncmC443BWR_250MBPSTo500MBPS == true) {
                        $scope.flag = "c";
                    }
                    if (promise.editlist[0].ncmC443BWR_50MBPSTo250MBPS == true) {
                        $scope.flag = "d";
                    }
                    if (promise.editlist[0].ncmC443BWR_LessThan50MBPS == true) {
                        $scope.flag = "e";
                    }
               
                    $scope.mI_Id = promise.editlist[0].mI_Id;
                }
            })

        }
    
        //=====================================================================

        $scope.change_institution = function () {
            $scope.NCMC443BWR_Year = '';
            $scope.NCMC443BWR_Range = '';
          
            $scope.NCMC443BWR_Id = 0;


           




        }
    }
})();

