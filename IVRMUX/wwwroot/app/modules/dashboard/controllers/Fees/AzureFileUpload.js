
(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeGroupsGroupingController', fee1)

    fee1.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$stateParams', '$q']
    function fee1($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $stateParams, $q) {
        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {

            return angular.lowercase(obj.fmG_GroupName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        var paginationformasters;
       
        $scope.savedisable = true;

        var pageid = 10;
    
    
        $scope.search = "";
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            
           
        }
        $scope.cance = function () {


            $state.reload();
        }
        $scope.submitted = false;
        $scope.saveYearlyGroupdata = function () {
            $scope.hrelT_SupportingDocument = '';
            $scope.UploadEmployeeDocumentBOSBOE();

            //if ($scope.hrelT_SupportingDocument != '') {
            //    swal("File Uploaded Successfully");
            //}
            //else {
            //    swal("Request Failed");
            //}
        };
       
        $scope.SelectedFileForUploadzdBOSBOE = [];
        $scope.selectFileforUploadzdBOSBOE = function (input) {
            $scope.SelectedFileForUploadzdBOSBOE = input.files;
            //$('#blah').removeAttr('src');
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                $scope.extention = nameArray[nameArray.length - 1];
              
   
            }
        };


        $scope.UploadEmployeeDocumentBOSBOE=function() {

            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadzdBOSBOE.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdBOSBOE[i]);
            }

            formData.append("Id", 0);

            var sample1 = Object.fromEntries(formData.entries());

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadhtmlfiles", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {
                        $scope.hrelT_SupportingDocument = d;
                        swal("File Uploaded Successfully");
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#documentid').removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }
 
    }
})();

