# vc-module-rating

# Features
 - Возможность подключать сколько угодно алгоритмов расчёта рейтинга. Реализовав свой вариант интерфейса IRatingCalculator и зарегестрировав его в DI модуль автоматически сможет использовать новый алгоритм, достаточно только в настройках стора его выбрать.
 
# Todo:
 - Убрать пустой блейд модуля в списке модулей, либо придумать что там разместить
 - Убрать не нужные настройки модуля
 - Интрнализация всего и вся
 
# How to check

 - заходим в настройки store Electronics и в группе Rating выбираем требуемый тип алгоритма расчёта рейтинга
 - в store Electronics кликаем на виджет Rating, и жмём кнопку Recalculate для перерасчёта всех ретингов данного Store
 - заходим в каталог Electronics -> Cell phones -> и открываем первые три телефона (только для них есть тестовые данные Review)
 - смотрим виджет Reting, при клике на него появляется список рейтингов данного продукта во всех Store
 