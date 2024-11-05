(function () {
    'use strict';
    angular
        .module('app')
        .controller('NAAC_HSU_Accreditation_424', NAAC_HSU_Accreditation_424)

    NAAC_HSU_Accreditation_424.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce', 'myFactorynaac'];
    function NAAC_HSU_Accreditation_424($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce, myFactorynaac) {

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
        $scope.NCHSUA424_Id = 0;
        $scope.instit = false;
        //======================page load
        $scope.loaddata = function () {
           
            $scope.NCHSUA424_Id = 0;
            $scope.change_institution();
            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }
            //var pageid = 2;
            apiService.getURI("NAAC_HSU_Accreditation_424/loaddata", $scope.mI_Id).then(function (promise) {

               
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

       
        //======================Record Save
        $scope.search = "";
        $scope.save = function () {
            debugger;
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
                    "NCHSUA424_Id": $scope.NCHSUA424_Id,
                    "asmaY_Id": $scope.NCHSUA424_Year,
                  
                    "flag": $scope.flag,
                    
                  
                    "MI_Id": $scope.mI_Id
                }

                apiService.create("NAAC_HSU_Accreditation_424/save", data).then(function (promise) {
                  
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
            debugger;
            var data = {
                "NCHSUA424_Id": user.nchsuA424_Id,
                "MI_Id": user.mI_Id
            }
            apiService.create("NAAC_HSU_Accreditation_424/EditData", data).then(function (promise) {
          
                if (promise.editlist.length > 0) {
                    $scope.instit = true;
                    $scope.NCHSUA424_Id = promise.editlist[0].nchsuA424_Id;
                    $scope.NCHSUA424_Year = promise.editlist[0].nchsuA424_Year;
                 

                    if (promise.editlist[0].nchsuA424_NabhAcrFlag == true) {
                        $scope.flag = "a";
                    }
                    if (promise.editlist[0].nchsuA424_NablAcrFlag == true) {
                        $scope.flag = "b";
                    }
                    if (promise.editlist[0].nchsuA424_IntAcrFlag == true) {
                        $scope.flag = "c";
                    }
                    if (promise.editlist[0].nchsuA424_ISOCertFlag == true) {
                        $scope.flag = "d";
                    }
                    if (promise.editlist[0].nchsuA424_GplorGcplFlag == true) {
                        $scope.flag = "e";
                    }
               
                    $scope.mI_Id = promise.editlist[0].mI_Id;
                }
            })

        }
    
        //=====================================================================

        $scope.change_institution = function () {
            $scope.NCHSUA424_Year = '';
            $scope.NCHSUA424_Id = 0;

        }
    }
})();

