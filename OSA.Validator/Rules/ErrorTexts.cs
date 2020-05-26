namespace OSA.Validator.Rules
{
    public static class ErrorTexts
    {
        public const string INCORRECT_FILLING = "Неправильное заполнение.";
        public const string MULTIVARIANT_VOTING_IS_PROHIBITED = "Многовариантное голосование недопустимо.";
        public const string NUMBER_OF_YES_CHECKES_SHOULD_NOT_EXCEED_PLACES_COUNT = "Превышено количество вариантов \"ЗА\".";
        public const string NOT_ALL_VOTES_WERE_DIVIDED_AMONG_FIELDS = "Распределены не все голоса.";
        public const string AT_LEAST_ONE_SELECTION_MUST_BE_MADE = "Не выбран ни один вариант голосования.";
        public const string PAGE_MUST_HAVE_SIGNATURE = "Нет подписи.";
        public const string CHECK_COOWNERS_SIGNATURES = "Бюллетень от " + CHECK_COONWERS_SIGNATURES_PLACEHOLDER + " совладельцев. Проверьте наличие доверенностей или подписей от всех совладельцев.";
        public const string CHECK_COONWERS_SIGNATURES_PLACEHOLDER = "#NumberOfSignatures#";
        public const string CANNOT_SPLIT_FRACTIONS_THIS_WAY = "Нельзя распределять дробную часть голосов таким образом.";
        public const string AMOUNT_OF_STOCK_SUBMITED_IS_GREATER_THAN_THERE_IS_ON_PACK = "Распределено большее число голосов, чем есть на пакете.";
        public const string WITH_MULTIPLE_CHECKS_VOTES_MUST_BE_SUBMITED = "При многовариантном голосовании должны быть распределены голоса.";
        public const string NO_TRUST_IN_DATABASE = "В базе данных отсутствует доверенность.";
        public const string CANNOT_USE_OTHER_CHECKS_WITH_HAVE_TRUST_CHECK = "С отметкой \"при наличии доверенности\" недопустимо использование других отметок.";
        public const string SELLER_PACKET_WITH_NOT_WHOLE_STOCK_PASSED_CHECK_MUST_HAVE_VOTES_SUBMITED = "У пакета продавца с отметкой \"переданы не все акции\" должны быть указны голоса.";
        public const string SELLER_PACK_CANNOT_HAVE_HAVE_TRUST_CHECK = "У пакета продавца не должно быть отметки \"при наличии доверенности\".";
        public const string SELLER_PACKET_MUST_HAVE_NOTWHOLESTOCKWASPASSED_CHECK = "У пакета продавца должна быть отметка \"переданы не все акции\" или отметка \"имею указания\"";
        public const string BUYERS_PACK_MUST_HAVE_HAVE_TRUST_CHECK = "У пакета покупателя должна быть проставлена отметка \"при наличии доверенности\".";
        public const string BUYERS_PACK_MUST_HAVE_VOTES_SUBMITED = "У пакета покупателя для каждого варианта голосования должно быть указано число голосов.";
        public const string BUYERS_PACK_CANNOT_HAVE_NOTWHOLESTOCKPASSED_CHECK = "У пакета покупателя не должно быть отметки \"если переданы не все акции\".";
        public const string CUMULATIVE_QUESTION_MUST_HAVE_VOTES_SUBMITED_IN_CASE_OF_YES_CHECK = "Для варианта голосования \"За\" Необходимо указать голоса.";
        public const string ADR_MUST_HAVE_EXACT_VOTES_AMOUNT_SPREADED = "АДР обязан распределить число голосов по вопросу, в точности соотвествующее числу заявленных голосов.";
        public const string ADR_MUST_HAVE_EXACT_VOTES_AMOUNT_SPREADED_OR_LESS_WITH_AT_LEAST_ONE_CANDIDATE_HAVING_EXACT_VOTES_AMOUNT = "АДР обязан распределить число голосов по вопросу, соотвествующее числу заявленных голосов или меньшее, при этом распределив заявленное как минимум по одному из кандидатов.";
        public const string PRIVILEGE_NO_ABS_VOTES_WILL_BE_IGNORED = "При голосовании по вопросу о выплате дивидендов по привилегированным акциям, привилегированные голоса, отданные за варианты 'ПРОТИВ' и 'ВОЗДЕРЖАЛСЯ', попадают в неучтенные.";
    }
}