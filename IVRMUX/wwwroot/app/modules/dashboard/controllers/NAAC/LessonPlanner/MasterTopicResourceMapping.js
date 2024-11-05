(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterTopicResourceMappingController', MasterTopicResourceMappingController)

    MasterTopicResourceMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q']
    function MasterTopicResourceMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.studentGuardianDetails = {};
        $scope.showbtn = false;

        var imagedownload = "";
        var studentreg = "";
        var docname = "";

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
                copty = "";
            }
        } else {
            paginationformasters = 10;
            copty = "";
        }

        $scope.coptyright = copty;

        $scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage1 = paginationformasters;
        $scope.currentPage = 1;


        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            var id = 2;
            $scope.ISMS_IdNew = "";
            $scope.subjectlist = [];
            $scope.LPMMT_IdNew = "";
            $scope.topicdetailsnews = [];
            $scope.grouptypeListOrder = [];
            $scope.topicdetailsnew = [];

            apiService.getURI("SchoolSubjectWithMasterTopicMapping/Getdetailsmapping", id).
                then(function (promise) {

                    $scope.subjectlist = promise.getsubjectlist;
                    angular.forEach($scope.subjectlist, function (dd) {
                        dd.ismS_SubjectName = dd.ismS_SubjectName + " : " + dd.ismS_SubjectCode;
                    });

                    $scope.getdetails = promise.getdetails;

                    angular.forEach($scope.getdetails, function (dd) {
                        var imgs = dd.lpmtR_Resources;
                        var imagarrd = imgs.split('.');
                        var lastelementd = imagarrd[imagarrd.length - 1];
                        dd.filetype = lastelementd;
                        if (dd.filetype === 'jpg') {
                            dd.filetype1 = 1;
                        }
                        else if (dd.filetype === 'pdf') {
                            dd.filetype1 = 3;
                        }
                        else if (dd.filetype === 'mp4') {
                            dd.filetype1 = 2;
                        }
                    });

                    //$scope.gridOptions.data = $scope.getdetails
                });

            ;
        };

        $scope.onchangesubject = function () {
            $scope.topicdetailsnew = [];
            var data = {
                "ISMS_Id": $scope.ISMS_Id.ismS_Id
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/onchangesubject", data).then(function (promise) {
                if (promise !== null) {
                    $scope.showbtn = true;
                    $scope.topicdetailsnew = promise.topicdetails;
                    $scope.grouptypeListOrder = promise.getorderdetails;
                    if ($scope.topicdetailsnew.length > 0) {
                        $scope.topicdetails = $scope.topicdetailsnew;

                    } else {
                        swal("No Topics Mapped To This Subjects");
                    }
                } else {
                    swal("Something Went Wrong , Kindly Contact Administrator");
                }
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.topicdetails.some(function (options) {
                return options.Selected;
            });
        };


        $scope.onchangetopic = function () {
            var data = {
                "ISMS_Id": $scope.ISMS_Id.ismS_Id,
                "LPMMT_Id": $scope.LPMMT_Id
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/onchangetopic", data).then(function (promise) {
                $scope.subtopicdetails = promise.subtopicdetails;
            });
        };

        $scope.onchangesubtopic = function () {
            var data = {
                "ISMS_Id": $scope.ISMS_Id.ismS_Id,
                "LPMMT_Id": $scope.LPMMT_Id,
                "LPMT_Id": $scope.LPMT_Id
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/onchangesubtopic", data).then(function (promise) {
                $scope.savedetails = promise.savedetails;

                angular.forEach($scope.savedetails, function (dd) {
                    var imgs = dd.lpmtR_Resources;
                    var imagarrd = imgs.split('.');
                    var lastelementd = imagarrd[imagarrd.length - 1];
                    dd.filetype = lastelementd;

                });
                if ($scope.savedetails.length > 0) {
                    $scope.studentGuardianDetails = $scope.savedetails;
                }

            });
        };




        // For File Upload

        $scope.studentGuardianDetails = [{ id: 'Guardian1' }];

        $scope.addNewsiblingguard = function () {
            var newItemNo = $scope.studentGuardianDetails.length + 1;

            if (newItemNo <= 10) {
                $scope.studentGuardianDetails.push({ 'id': 'Guardian' + newItemNo });
            }
        };

        $scope.removeNewsiblingguard = function (index) {
            var newItemNo = $scope.studentGuardianDetails.length - 1;
            $scope.studentGuardianDetails.splice(index, 1);

            if ($scope.studentGuardianDetails.length === 0) {
                //data
            }
        };

        $scope.UploadGuardianPhoto = [];

        $scope.uploadGuardianPhoto = function (input, document) {

            $scope.UploadGuardianPhoto = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type !== "video/mp4") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type !== "application/pdf") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload MP4, Pdf, Image Files Only");
                }

            }
        };

        function UploaddianPhoto(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadGuardianPhoto.length; i++) {
                formData.append("File", $scope.UploadGuardianPhoto[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/lessonplannerdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.lpmtR_Resources = d;
                    $('#').attr('src', data.lpmtR_Resources);
                    var img = data.lpmtR_Resources;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    // swal(d);
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

            // Uploads1(miid);
        }

        $scope.showGuardianPhoto = function (data) {
            imagedownload = data.lpmtR_Resources;
            studentreg = data.ismS_SubjectName;
            docname = data.lpmmT_TopicName + ' ' + data.lpmT_TopicName;

            $('#preview').attr('src', data.lpmtR_Resources);
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.videoSources = [];
            $scope.preview1 = data.lpmtR_Resources;
            $scope.videdfd = data.lpmtR_Resources;
            $scope.movie = { src: data.lpmtR_Resources };
            $scope.movie1 = { src: data.lpmtR_Resources };
            console.log($scope.movie);
        };



        // TO Save The Data
        $scope.submitted = false;

        $scope.saveddata = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {

                if ($scope.studentGuardianDetails.length === 0) {
                    swal("Upload Atleast One File To Save Details");
                    return;
                }

                var data = {
                    "LPMT_Id": $scope.LPMT_Id,
                    "SchoolSubjectWithMasterTopicResourceMappingTempDTO": $scope.studentGuardianDetails
                };

                apiService.create("SchoolSubjectWithMasterTopicMapping/savemapping", data).
                    then(function (promise) {
                        // $scope.newuser = promise.mastersubexam;

                        if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal('Record saved successfully');
                            } else {
                                swal('Failed To save Record');
                            }
                        } else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal('Record updated successfully');
                            } else {
                                swal('Failed To Update Record');
                            }
                        }

                        else if (promise.message === 'Duplicate') {
                            swal('Record already exist');
                        }
                        else {
                            swal("Failed To Save / Update Record");
                        }
                        $scope.cancel();
                        //$scope.BindData();
                    });
            } else {
                $scope.submitted = true;
            }

        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.search = "";
        $scope.filterValuesearch = function (obj12) {
            return (angular.lowercase(obj12.ismS_SubjectName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj12.lpmmT_TopicName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj12.lpmT_TopicName)).indexOf(angular.lowercase($scope.search)) >= 0
        };


        //Table 
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'ismS_SubjectName', displayName: 'Subject Name' },
                { name: 'lpmmT_TopicName', displayName: 'Topic Name' },
                { name: 'lpmT_TopicName', displayName: 'Sub Topic' },
                {
                    name: 'lpmtR_Resources', displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:

                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.filetype1 == 1"   href="javascript:void(0)" data-toggle="modal" data-target="#myModal" data-backdrop="static" ng-click="grid.appScope.showGuardianPhoto(row.entity);"> <i class="fa fa-picture-o text-green" ></i></a> ' +
                        '<a ng-if="row.entity.filetype1 == 2"  href="javascript:void(0)" data-toggle="modal" data-target="#popup15" data-backdrop="static" ng-click="grid.appScope.showGuardianPhotonew(row.entity);"> <i class="fa fa-film text-blue" ></i></a> ' +
                        '<a ng-if="row.entity.filetype1 == 3"  href="row.entity.lpmtR_Resources" onclick="DownloadFile(row.entity); target="_blank"> <i class="fa fa-file-pdf-o" ></i></a>' +
                        '</div>'
                }
            ],

            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }
        };


        $scope.download = function () {
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
    }




})();

app.filter('trusted', ['$sce', function ($sce) {
    return function (url) {
        console.log(url);
        return $sce.trustAsResourceUrl(url);
    };
}]);