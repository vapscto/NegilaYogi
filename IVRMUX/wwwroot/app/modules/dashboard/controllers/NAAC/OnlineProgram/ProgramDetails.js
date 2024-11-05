(function () {
    'use strict';
    angular
        .module('app')
        .controller('ProgramDetailsController', ProgramDetailsController)

    ProgramDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q']
    function ProgramDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q) {


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

            apiService.getURI("ProgramDetails/getloaddata", id).
                then(function (promise) {

                    $scope.programlist = promise.programlist;
                    $scope.getdetails = promise.alllist;


                    if ($scope.getdetails.length > 0) {
                        $scope.studentDetails = $scope.getdetails;
                    }


                    angular.forEach($scope.getdetails, function (dd)   {
                        var imgs = dd.PRYRP_Photos;
                        var imagarrd = imgs.split('.');
                        var lastelementd = imagarrd[imagarrd.length - 1];
                        
                        var vids = dd.PRYRV_Videos;
                        var vidarrd = vids.split('.');
                        var lastelementdvid = vidarrd[imagarrd.length - 1];

                        dd.filetype1 = lastelementd;
                        dd.filetype2 = lastelementdvid;
                        if (dd.filetype1 === 'jpg') {
                            dd.filetype = 1;
                        }
                        if (dd.filetype2 === 'mp4') {
                            dd.filetype = 2;
                        }
                    });

                    //$scope.gridOptions.data = $scope.getdetails
                });

            ;
        };


        //$scope.getStudentBYYear = function () {
        //    $scope.studentGuardianDetails = [];
        //    angular.forEach($scope.getdetails, function (dd) {
        //        if (dd.PRYR_Id == $scope.pryR_Id) {

        //            var imgs = dd.PRYRP_Photos;
        //            var imagarrd = imgs.split('.');
        //            var lastelementd = imagarrd[imagarrd.length - 1];
        //            dd.lpmtR_Resources = imgs;

        //            var vids = dd.PRYRV_Videos;
        //            var vidarrd = vids.split('.');
        //            var lastelementdvid = vidarrd[imagarrd.length - 1];

        //            dd.filetype1 = lastelementd;
        //            dd.filetype2 = lastelementdvid;

        //            $scope.studentGuardianDetails.push(dd);
                    
        //        }
        //        //if ($scope.getdetails.length > 0) {
        //        //    $scope.studentGuardianDetails = $scope.getdetails;
        //        //}
        //    });
           
        //};


        $scope.DeletRecord = function (employee, SweetAlert) {
            $scope.editEmployee = employee.PRYR_Id;
            var pageid = $scope.editEmployee;
            swal({
                title: "Are you sure",
                text: "Do you want to delete record????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("ProgramDetails/deleterecord", pageid).
                            then(function (promise) {
                                if (promise.returnresult === true) {
                                    swal('Record Deleted Successfully');
                                }
                                else {
                                    swal('Record Not Deleted Successfully!');
                                }

                            })

                    }
                    else {
                        swal("Record Deletion Cancelled");
                        $state.reload();
                    }
                    $state.reload();
                });
        };








        $scope.isOptionsRequired = function () {
            return !$scope.topicdetails.some(function (options) {
                return options.Selected;
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
                else if (input.files[0].type === "video/mp4") {
                    UploaddianPhoto(document);
                }
            
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload MP4,  Image Files Only");
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
            $http.post("/api/ImageUpload/OnlineProgramdoc", formData,
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
            imagedownload = data.PRYRP_Photos;
            studentreg = data.PRYR_ProgramName;
            docname = data.PRYR_ProgramName;

            $('#preview').attr('src', data.PRYRP_Photos);
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.videoSources = [];
            $scope.preview1 = data.PRYRV_Videos;
            $scope.videdfd = data.PRYRV_Videos;
            $scope.movie = { src: data.PRYRV_Videos };
            $scope.movie1 = { src: data.PRYRV_Videos };
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
                    "PRYR_Id": $scope.pryR_Id,
                    "pgTempDTO": $scope.studentGuardianDetails,
                };

                apiService.create("ProgramDetails/savedetail2", data).
                    then(function (promise) {
                   
                        if (promise.message === "Add") {
                            if (promise.returnresult === true) {
                                swal('Record saved successfully');
                            } else {
                                swal('Failed To save Record');
                            }
                        } else if (promise.message === "Update") {
                            if (promise.returnresult === true) {
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
        //$scope.filterValuesearch = function (obj12) {
        //    return (angular.lowercase(obj12.pryR_ProgramName)).indexOf(angular.lowercase($scope.search)) >= 0 
        //};


        ////Table 
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'pryR_ProgramName', displayName: 'Program Name' },
               
                {
                    name: 'PRYRV_Videos', displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:

                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.filetype == 1"   href="javascript:void(0)" data-toggle="modal" data-target="#myModal" data-backdrop="static" ng-click="grid.appScope.showGuardianPhoto(row.entity);"> <i class="fa fa-picture-o text-green" ></i></a> '+
                        '</div>'
                },

                  {
                      name: 'PRYRP_Photos', displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:

                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.filetype == 2"  href="javascript:void(0)" data-toggle="modal" data-target="#popup15" data-backdrop="static" ng-click="grid.appScope.showGuardianPhotonew(row.entity);"> <i class="fa fa-film text-blue" ></i></a> ' +
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