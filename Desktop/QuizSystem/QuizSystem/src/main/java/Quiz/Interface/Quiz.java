package Quiz.Interface;

import java.util.List;

public interface Quiz {
    String getTitle();                       // Quiz başlığı
    List<? extends Question> getQuestions(); // Soru listesi
}