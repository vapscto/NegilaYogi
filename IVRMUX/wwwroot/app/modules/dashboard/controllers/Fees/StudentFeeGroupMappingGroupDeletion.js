(function () {
    'use strict';
    angular
        .module('app')
        .controller('StudentFeeGroupMappingGroupDeletionController', StudentFeeGroupMappingGroupDeletionController)

    StudentFeeGroupMappingGroupDeletionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function StudentFeeGroupMappingGroupDeletionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.studentdetails = [];
     
 
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));


        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.sortKey1 = "fmsG_Id";   //set the sortKey to the param passed
        $scope.reverse1 = true; //if true make it false and vice versa
        $scope.totcountsearch = 0;
        $scope.totcountsearch_s = 0;
        $scope.totcountsearch_o = 0;


        $scope.page1 = "page1";
        $scope.page2 = "page2";


        $scope.reverse1 = true;
        $scope.reverse2 = true;


        $scope.formload = function () {
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = paginationformasters

            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = paginationformasters



            var pageid = 1;

            apiService.getURI("StudentFeeGroupMappingGroupDeletion/getalldetails", pageid).
                then(function (promise) {

                    $scope.yearlst = promise.academicdrp;

                    $scope.ASMAY_Id = academicyrlst[0].asmaY_Id;
                    $scope.classcount = promise.fillmasterclass;
                    $scope.sectiondrpre = promise.fillmastersection;
                    $scope.grouplst = promise.fillmastergroup;












                })







        }


        $scope.Getreport = function () {

            if ($scope.sectiondrp == "all") {
                var secid = [];

                angular.forEach($scope.sectiondrpre, function (ty) {

                    secid.push(ty.asmS_Id);

                })
            }

            if ($scope.ASMCL_Id == "all") {
            var classid = [];
         
            angular.forEach($scope.classcount, function (ty) {
              
                classid.push(ty.asmcL_Id);
                
            })
            }
            if ($scope.sectiondrp == "all" && $scope.ASMCL_Id == "all") {
                var data = {


                    "ASMAY_Id": $scope.ASMAY_Id,
                    "FMG_Id": $scope.FMG_Id,

                    ASMS_Ids: secid,

                    ASMCL_Ids: classid,
                    flag:1
                }
            }
            else if ($scope.sectiondrp == "all" && $scope.ASMCL_Id != "all") {
                var data = {


                    "ASMAY_Id": $scope.ASMAY_Id,
                    "FMG_Id": $scope.FMG_Id,

                    ASMS_Ids: secid,

                    "ASMCL_Id": $scope.ASMCL_Id,
                    flag:2
                }
            }
            else if ($scope.sectiondrp != "all" && $scope.ASMCL_Id == "all") {
                var data = {


                    "ASMAY_Id": $scope.ASMAY_Id,
                    "FMG_Id": $scope.FMG_Id,

                    "ASMS_Id": $scope.sectiondrp,


                    ASMCL_Ids: classid,
                    flag:3
                }
            }
            else {

                var data = {


                    "ASMAY_Id": $scope.ASMAY_Id,
                    "FMG_Id": $scope.FMG_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.sectiondrp,
                    flag:4
                }
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }



            apiService.create("StudentFeeGroupMappingGroupDeletion/Getreport", data).
                then(function (promise) {
                    if (promise.alldatathird.length > 0) {
                        $scope.cartgrid = promise.alldatathird;
                    }
                    else {
                        swal("No Record Found");
                        $scope.cartgrid = [];
                    }

                })

        }
        $scope.selectacademicyear = function (yearlst) {




            var data = {


                "ASMAY_Id": $scope.ASMAY_Id


            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }



            apiService.create("StudentFeeGroupMappingGroupDeletion/onclickClass", data).
                then(function (promise) {

                    $scope.cartgrid = promise.fillmastergroup;

                })

        }

        $scope.addcart = function () {
            angular.forEach($scope.cartgrid, function (itm) {
                if (itm.studentchecked == true) {
                    //if ($scope.studentdetails.length == 0) {
                        $scope.studentdetails.push(itm);
                    //}
                    //else {
                    //    angular.forEach($scope.thirdgrid, function (item) {


                    //        if (item.fmG_Id == itm.fmG_Id && item.amsT_Id == itm.amsT_Id && item.fmsG_Id == itm.fmsG_Id) {

                    //        }
                    //        else {
                    //            $scope.studentdetails.push(itm);

                    //        }

                    //    });
                    //}



                }
                
             
    });

            $scope.thirdgrid = $scope.studentdetails;
            $scope.cartgrid = [];

}

        $scope.DeletRecord = function () {

            $scope.studentdetails = [];
    angular.forEach($scope.thirdgrid, function (itm) {
        if (itm.studchecked == true) {


            $scope.studentdetails.push(itm);

        }
    });

    var data = {
        studentdata: $scope.studentdetails,

        "ASMAY_Id": $scope.ASMAY_Id
    }

    var config = {
        headers: {
            'Content-Type': 'application/json;'
        }
    }


    swal({
        title: "Are you sure?",
        text: "Do You Want To Delete Record?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
        cancelButtonText: "Cancel",
        closeOnConfirm: false,
        closeOnCancel: false
    },
        function (isConfirm) {
            if (isConfirm) {
                apiService.create("StudentFeeGroupMappingGroupDeletion/Deletedetails", data).
                    then(function (promise) {

                        // $scope.thirdgrid = promise.alldata;

                        if (promise.returnval == "true") {

                            //$scope.masterse = promise.masterSectionData;

                            swal('Record Deleted Successfully');
                            $scope.thirdgrid = [];
                        }
                        else {
                            swal('Record cannot be Deleted.Transaction has already been done for this group');
                        }
                        $state.reload();
                    })
            }
            else {
                swal("Record Deletion Cancelled");
            }
        });
}

$scope.toggleAllstu = function (allchkdatastudent) {

    if (allchkdatastudent == true) {
        var toggleStatusstu = $scope.selectedAllstu;
        angular.forEach($scope.thirdgrid, function (itm) {
            itm.studchecked = toggleStatusstu;
        });




    }
    else {

        angular.forEach($scope.thirdgrid, function (itm) {
            itm.studchecked = false;
        });


    }

}
$scope.toggleAllstucart = function (allchkdatastu) {

    if (allchkdatastu == true) {
        var toggleStatusstu = $scope.selectedAllstudent;
        angular.forEach($scope.cartgrid, function (itm) {
            itm.studentchecked = toggleStatusstu;
        });




    }
    else {

        angular.forEach($scope.cartgrid, function (itm) {
            itm.studentchecked = false;
        });


    }

}
    }


}) ();