package Quiz.Interface;

import java.util.List;

public interface Question {
    String getPrompt();                   // Soru metni
    List<? extends Answer> getAnswers();  // Cevap seçenekleri
    int getCorrectAnswerIndex();          // Doğru cevabın indeksi
}