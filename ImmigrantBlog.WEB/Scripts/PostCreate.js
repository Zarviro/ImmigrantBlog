

        $(document).ready(function () {
            // Jquery UI
            //*******************************************

            // autocomplite
            $("[data-autocomplete-source]").each(function () {
                var target = $(this);
                target.autocomplete({ source: target.attr("data-autocomplete-source") });
            });
            // combobox
            $("#selected_tag").combobox();
            $(".tags .custom-combobox-input").val(""); // reset value
            $("#categories").selectmenu();
            // tinymce
            tinymce.init({
                elements: "short_description, description",
                mode: "exact",
                language: "ru",
                plugins: [
                  'advlist autolink lists link image charmap print preview anchor',
                  'searchreplace visualblocks code fullscreen',
                  'insertdatetime media table contextmenu paste code'
                ],
                toolbar: 'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
                statusbar: false,
            });



            // Events
            //*******************************************

            // disable enter form submit
            $(document).on('keyup keypress', 'form input', function (e) {
                if (e.which == 13) {
                    e.preventDefault();
                    return false;
                }
            });

            // add tag
            $("#selected_tag").combobox({
                select: function (event, ui) {
                    var tagId = $("#selected_tag").val();
                    var tag = $("#selected_tag option:selected").text();
                    var li = $("<li>").attr("id", "tag_id_" + tagId).attr("class", "search-choice collective");
                    li.append($("<span>").attr("class", "text").append(tag));
                    li.append($("<a>").attr("href", "#").attr("class", "search-choice-close").attr("rel", tagId).append("| x"));
                    li.append($("<input />", { type: 'checkbox', value: tagId, name: "selectedTags", checked: "checked"}).attr("style", "display: none"));
                    li.insertBefore($(".tags .search-field"));
                    // очищать надо после задержки времени
                    setTimeout(function () {
                        $(".tags .custom-combobox-input").val("");
                        // удаляем выбранный тег из списка
                        $("#selected_tag option:selected").remove();
                    }, 50);
                }
            });

            // delete tag
            $(".tags").on("click", ".search-choice-close", function (event) {
                event.preventDefault();
                var removeLi = "#tag_id_" + $(this).attr("rel");
                // добавляем тег снова в список
                var tagId = $(this).attr("rel");
                var tag = $(removeLi + " span").text();
                $("#selected_tag").append($("<option>", {
                    value: tagId,
                    text: tag
                }));
                // сортируем
                var sel = $("#selected_tag");
                var opts_list = sel.find("option");
                opts_list.sort(function (a, b) { return $(a).text() > $(b).text() ? 1 : -1; });
                sel.html("").append(opts_list);
                // удаляем li
                $(removeLi).remove();
            });



            // Function
            //*******************************************

            // hidden
            function hidden() {
                return this.each(function () {
                    $(this).css("visibility", "hidden");
                });
            };
            // visible
            function visible() {
                return this.each(function () {
                    $(this).css("visibility", "visible");
                });
            };
        });
