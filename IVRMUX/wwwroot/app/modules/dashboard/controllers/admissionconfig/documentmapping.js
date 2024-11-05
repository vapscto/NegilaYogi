//dashboard.controller("attendanceEntryTypeController", ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash',
//function ($rootScope, $scope, $state, $location, dashboardService, Flash) {



//}]);



(function () {
    'use strict';
    angular
.module('app')
.controller('documentmappingController', documentmappingController)

    documentmappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function documentmappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.BindData = function () {
            apiService.getDATA("MasterDocumentMapping/Getdetails").
       then(function (promise) {
           $scope.gridviewDetails = promise.gridviewDetails;
           $scope.documentList = promise.documentList;
           $scope.categoryList = promise.categoryList;
       })
        };

        $scope.Categorycheckboxchcked = [];

        $scope.CheckedCategoryName = function (data) {

            if ($scope.Categorycheckboxchcked.indexOf(data) === -1) {
                $scope.Categorycheckboxchcked.push(data);
            }
            else {
                $scope.Categorycheckboxchcked.splice($scope.Categorycheckboxchcked.indexOf(data), 1);
            }
        }

        $scope.Documentcheckboxchcked = [];

        $scope.CheckedDocumentName = function (data) {

            if ($scope.Documentcheckboxchcked.indexOf(data) === -1) {
                $scope.Documentcheckboxchcked.push(data);
            }
            else {
                $scope.Documentcheckboxchcked.splice($scope.Documentcheckboxchcked.indexOf(data), 1);
            }
        }

        $scope.Deletedata = function (DeleteRecord) {
            var confirmPopup = confirm('Are you sure you want to delete this item?');
            if (confirmPopup === true) {
                $scope.deleteId = DeleteRecord.pascD_Id;
                var MdeleteId = $scope.deleteId;
                apiService.DeleteURI("MasterDocumentMapping/DeleteData", MdeleteId)

                $scope.$apply();

                swal("Record Deleted Successfully");
                $scope.saved = "Record Deleted Successfully";

                $scope.BindData();

            }

        };

        $scope.Editdata = function (EditRecord) {
            $scope.EditId = EditRecord.pascD_Id;
            var MEditId = $scope.EditId;
            apiService.getURI("MasterDocumentMapping/GetSelectedRowDetails/", MEditId).
            then(function (promise) {

                $scope.documentList = promise.documentList;
                $scope.categoryList = promise.categoryList;


            })
        };


        $scope.savedata = function () {

            var CategoryIDs = $scope.Categorycheckboxchcked;
            
            var DocIDs = $scope.Documentcheckboxchcked;

            var data = {
                "SelCategoryList": CategoryIDs,
                "SelDocumentList": DocIDs
            }

            apiService.create("MasterDocumentMapping/", data)

            $scope.$apply();

            swal('Record Saved Successfully')
            $scope.saved = "Record Saved Successfully";

            $scope.BindData();


        };

        $scope.cancel = function () {
            $scope.name = "";
            $scope.description = "";
        };

    }

})();