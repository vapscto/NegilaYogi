(function () {
    'use strict';
    angular
        .module('app')
        .controller('PdfConverter', rohan);

    rohan.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', '$window', 'Excel', '$timeout'];

    function rohan($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, $window, Excel, $timeout) {

        var fileInput = "";
        var filename = "";
        var file;
        var fileInput = document.getElementById('fileInput');
        fileInput.onchange = function () {
            file = fileInput.files[0];
            var filename = file.name;
            document.getElementById("uploadPreview").src = URL.createObjectURL(file);
            document.getElementById('fname').textContent = filename;
        }

        //to preview the file
        var previewlink
        fileInput = document.getElementById('fileInput');
        $scope.previewfile = function () {
            file = fileInput.files[0];
            previewlink = URL.createObjectURL(file);
            window.open(previewlink);
        }

        $scope.cancel = function () {
            // Clear the file input field
            fileInput.value = '';

            // Clear the filename
            filename = '';

            // Optionally, you can clear the file preview
            document.getElementById("uploadPreview").src = '';
            document.getElementById('fname').textContent = '';
        };


        $scope.convertToPDF = function () {
            var fileInput = document.getElementById('fileInput');
            var file = fileInput.files[0];
            var fileType = file ? file.type : null;

            if (file) {
                if (fileType == 'text/plain') {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        var textContent = e.target.result;
                        var pdfContainer = document.createElement('div');
                        pdfContainer.innerText = textContent;

                        html2pdf(pdfContainer, {
                            margin: 5,
                            filename: file.name.replace(/\.[^/.]+$/, "") + '.pdf',
                            image: { type: 'jpeg', quality: 2 },
                            html2canvas: { scale: 2 },
                            jsPDF: { unit: 'mm', format: 'a4', orientation: 'portrait' }
                        });
                    };

                    reader.readAsText(file);
                } else if (fileType == 'image/png' || fileType == 'image/jpeg') {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        try {
                            var content = e.target.result;
                            var fileType = file.type;

                            var docDefinition;

                            if (fileType.includes('image')) {
                                docDefinition = {
                                    content: [
                                        {
                                            image: content,
                                            fit: [500, 500],
                                        }
                                    ]
                                };
                            } else if (fileType === 'application/pdf') {
                                docDefinition = {
                                    content: [
                                        { text: 'PDF content goes here.' }
                                    ]
                                };
                            }

                            createAndDownloadPDF(docDefinition); // Call the function to create and download PDF
                        } catch (error) {
                            $scope.errorMessage = 'Error reading the file.';
                        }
                    };

                    reader.readAsDataURL(file);
                } else {
                    swal("Please select a valid file", "", "warning");
                }
            } else {
                swal("Please select a file", "", "warning");
            }
        };
        function createAndDownloadPDF(docDefinition) {
            var pdfDoc = pdfMake.createPdf(docDefinition);

            // Automatically download the PDF after it's generated
            pdfDoc.download('generated.pdf', function () {
                $scope.errorMessage = '';
            });
        }
          
        function createAndDownloadPDF(docDefinition) {
            var pdfDoc = pdfMake.createPdf(docDefinition);

            // Automatically download the PDF after it's generated
            pdfDoc.download('generated.pdf', function () {
                $scope.errorMessage = '';
            });
        }             
    }
})();
