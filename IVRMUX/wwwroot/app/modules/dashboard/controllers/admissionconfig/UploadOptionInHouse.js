(function () {
    'use strict';
    angular.module('app').controller('admissionformuploadController', admissionformuploadController)
    admissionformuploadController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$stateParams']
    function admissionformuploadController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $stateParams) {
                 
        
        $scope.UploadStudentProfilePic = [];

        $scope.uploadStudentProfilePic = function (input, document) {
            $scope.UploadStudentProfilePic = input.files;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#blah').attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();

                }
                else if (input.files[0].type !== "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        };

        function Uploadprofile() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadStudentProfilePic.length; i++) {
                formData.append("File", $scope.UploadStudentProfilePic[i]);
            }
            //We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadinhouseDTo", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    console.log(d);
                    $scope.uploadimg = d;
                    $('#sfd').attr('src', '@'+$scope.uploadimg);
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }

        var imagedownload = "";
        var docname = "";
        $scope.downloaddirectimage = function (data, idd, typeofphoto) {
            var studentreg = idd;
            $scope.imagedownload = data;
            imagedownload = data;
            docname = typeofphoto;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '-' + docname + '.jpg'
                    })[0].click();
                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.clearAll = function () {
            $state.reload();
        };

        $scope.reload = function () {
            $state.reload();
        };

        $scope.zoomin = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth >= 750) {
                swal("Maximum zoom-in level reached.");
            } else {
                myImg.style.width = (currWidth + 50) + "px";
            }
        };

        $scope.zoomout = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth <= 400) {
                swal("Maximum zoom-out level reached.");
            } else {
                myImg.style.width = (currWidth - 50) + "px";
            }
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };
    }
})();