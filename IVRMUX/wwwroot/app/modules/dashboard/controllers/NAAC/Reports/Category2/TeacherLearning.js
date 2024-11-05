(function () {
    'use strict';
    angular
        .module('app')
        .controller('TeacherLearningController', TeacherLearningController)

    TeacherLearningController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function TeacherLearningController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {


        $scope.ListofItems = [];
        $scope.ListofItems = [
            { name: "BC0031", program: "BE0023", certificateName: "Automation Certificate", year: "2018-2019", course: "BB0090" },
            { name: "BC0032", program: "BE002", certificateName: " Certificate-D", year: "2017-2018", course: "BB0088" },
            { name: "BC0033", program: "BE0021", certificateName: " Certificate-C", year: "2016-2017", course: "BB0092" },
            { name: "BC0034", program: "BE0025", certificateName: " Certificate-B", year: "2015-2016", course: "BB0077" },
            { name: "BC0035", program: "BE0032", certificateName: " Certificate-A", year: "2014-2015", course: "BB0082" },
        ]

        $scope.exportToExcel = function (table) {

            var excelname = 'Cat 2.2.xls';
            var exportHref = Excel.tableToExcel(table, '2.2.3');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        }

        //-----------upload file/photo.............
        $scope.uploadFile = function (input, document) {

            $scope.UploadFile = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" || input.files[0].type == "image/png" || input.files[0].type == "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/doc" || input.files[0].type === "application/docx" || input.files[0].type === "application/vnd.ms-excel" &&
                    input.files[0].size <= 2097152) {

                    var reader = new FileReader();

                    reader.onload = function (e) {

                        $('#blahD')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        }
        function Uploadprofile() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadFile.length; i++) {

                formData.append("File", $scope.UploadFile[i]);
                $scope.file_detail = $scope.UploadFile[0].name;
            }
            //We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_Noticefiles", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    $scope.notice = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
        //----------End..........!



        ///=========clear upload field data......
        $scope.remove_file = function () {

            $scope.file_detail = "";
            $scope.notice = "";
        }


    }
})();

