package com.udj.labfour.springinjection.springinjectionjava;

import com.udj.labfour.springinjection.springinjectionjava.domain.Person;
import com.udj.labfour.springinjection.springinjectionjava.service.PersonService;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;

@SpringBootTest
class SpringInjectionJavaApplicationTests {

	@Autowired
	PersonService ps;

	@Test
	void contextLoads() {
	}

	@Test
	public void addPersonToList(){
		//given
		Person person = new Person("Andrzej","Elektryk",25);

		//when
		ps.addPerson(person);
		//then
		assert(ps.getPerson()).equals(person);
	}

	@Test
	void deletePersonFromList(){
		//given
		Person person = new Person("Andrzej","Elektryk",30);
		ps.addPerson(person);

		//when
		ps.deletePerson(person);

		//then
		assert(ps.getPersons().isEmpty());
	}



}
